using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using log4net;
using Microsoft.Practices.Unity;
using WebMoney.Services.BusinessObjects;
using WebMoney.Services.Contracts;
using WebMoney.Services.Contracts.BasicTypes;
using WebMoney.Services.Contracts.BusinessObjects;
using WebMoney.Services.DataAccess.EF;
using WebMoney.Services.ExternalServices.Contracts;

namespace WebMoney.Services
{
    public sealed class TransferBundleService : SessionBasedService, ITransferBundleService
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(TransferBundleService));

        public int RegisterBundle(IEnumerable<IOriginalTransfer> transfers, string name)
        {
            if (null == transfers)
                throw new ArgumentNullException(nameof(transfers));

            if (null == name)
                throw new ArgumentNullException(nameof(name));

            var transferList = transfers.ToList();

            if (0 == transferList.Count)
                throw new ArgumentOutOfRangeException(nameof(transfers));

            if (!Session.AuthenticationService.HasConnectionSettings)
                throw new InvalidOperationException("!Session.AuthenticationService.HasConnectionSettings");

            string sourceAccountName = transferList.First().SourcePurse;

            if (null == sourceAccountName)
                throw new ArgumentException("null == sourceAccountName");

            foreach (var originalTransfer in transferList)
            {
                if (!sourceAccountName.Equals(originalTransfer.SourcePurse))
                    throw new ArgumentException("!sourceAccountName.Equals(originalTransfer.SourcePurse)");
            }

            var preparedTransfers = transferList.Select(
                    t => new PreparedTransfer(t.PaymentId, t.SourcePurse, t.TargetPurse, t.Amount, t.Description))
                .ToList();

            var totalAmount = preparedTransfers.Sum(t => t.Amount);

            var transferBundle =
                new TransferBundle(Session.CurrentIdentifier, name, sourceAccountName, transferList.Count, totalAmount)
                {
                    Name = name
                };
            transferBundle.Transfers.AddRange(preparedTransfers);

            using (var context = new DataContext(Session.AuthenticationService.GetConnectionSettings()))
            {
                context.TransferBundles.Add(transferBundle);
                context.SaveChanges();
            }

            return transferBundle.Id;
        }

        public IEnumerable<ITransferBundle> SelectBundles(DateTime fromTime, DateTime toTime)
        {
            if (!Session.AuthenticationService.HasConnectionSettings)
                throw new InvalidOperationException("!Session.AuthenticationService.HasConnectionSettings");

            long identifier = Session.CurrentIdentifier;

            List<TransferBundle> transferBundles;

            using (var context = new DataContext(Session.AuthenticationService.GetConnectionSettings()))
            {
                transferBundles = (from tb in context.TransferBundles
                    where tb.Identifier == identifier && tb.CreationTime >= fromTime && tb.CreationTime <= toTime
                    orderby tb.Id descending
                    select tb).ToList();
            }

            return transferBundles;
        }

        public ITransferBundle ObtainBundle(int bundleId, bool includeTransfers)
        {
            if (!Session.AuthenticationService.HasConnectionSettings)
                throw new InvalidOperationException("!Session.AuthenticationService.HasConnectionSettings");

            TransferBundle transferBundle;

            using (var context = new DataContext(Session.AuthenticationService.GetConnectionSettings()))
            {
                var q = includeTransfers
                    ? context.TransferBundles.Include("Transfers").OrderByDescending(t => t.CreationTime)
                    : context.TransferBundles;

                transferBundle = (from entity in q
                    where entity.Id == bundleId
                    select entity).First();
            }

            return transferBundle;
        }

        public IPreparedTransfer ObtainPreparedTransfer(int id)
        {
            if (!Session.AuthenticationService.HasConnectionSettings)
                throw new InvalidOperationException("!Session.AuthenticationService.HasConnectionSettings");

            PreparedTransfer preparedTransfer;

            using (var context = new DataContext(Session.AuthenticationService.GetConnectionSettings()))
            {
                preparedTransfer = context.PreparedTransfers.FirstOrDefault(t => t.Id == id);
            }

            if (null == preparedTransfer)
                throw new InvalidOperationException("null == preparedTransfer");

            return preparedTransfer;
        }

        public void ProcessBundleAsync(int bundleId)
        {
            if (!Session.AuthenticationService.HasConnectionSettings)
                throw new InvalidOperationException("!Session.AuthenticationService.HasConnectionSettings");

            TransferBundle transferBundle;
            List<PreparedTransfer> updatedTransfers;

            using (var context = new DataContext(Session.AuthenticationService.GetConnectionSettings()))
            {
                var transaction = context.Database.BeginTransaction(IsolationLevel.ReadCommitted);

                try
                {
                    LockBundle(context, bundleId);

                    transferBundle = context.TransferBundles.Include("Transfers").First(tb => tb.Id == bundleId);

                    if (TransferBundleState.Registered == transferBundle.State)
                    {
                        updatedTransfers = Start(transferBundle);
                        context.SaveChanges();
                        transaction.Commit();
                    }
                    else
                    {
                        transferBundle = null;
                        updatedTransfers = new List<PreparedTransfer>();

                        transaction.Rollback();
                    }
                }
                catch (Exception exception)
                {
                    transaction.Rollback();
                    Logger.Error(exception.Message, exception);

                    throw;
                }
            }

            // Notification
            if (null != transferBundle)
            {
                var eventBroker = Container.Resolve<IEventBroker>();

                eventBroker.Notify(new TransferBundleNotification(transferBundle));

                foreach (var updatedTransfer in updatedTransfers)
                {
                    eventBroker.Notify(new PreparedTransferNotification(updatedTransfer));
                }

                Container.Resolve<ITransferBundleProcessor>().RunAsync();
            }
        }

        public void AbortProcessingAsync(int bundleId)
        {
            if (!Session.AuthenticationService.HasConnectionSettings)
                throw new InvalidOperationException("!Session.AuthenticationService.HasConnectionSettings");

            TransferBundle transferBundle;
            List<PreparedTransfer> updatedTransfers;

            using (var context = new DataContext(Session.AuthenticationService.GetConnectionSettings()))
            {
                var transaction = context.Database.BeginTransaction(IsolationLevel.ReadCommitted);

                try
                {
                    LockBundle(context, bundleId);

                    transferBundle = context.TransferBundles.Include("Transfers").First(tb => tb.Id == bundleId);

                    if (TransferBundleState.Pended == transferBundle.State
                        || TransferBundleState.Processed == transferBundle.State)
                    {
                        updatedTransfers = Stop(transferBundle);
                        context.SaveChanges();
                        transaction.Commit();
                    }
                    else
                    {
                        transferBundle = null;
                        updatedTransfers = new List<PreparedTransfer>();

                        transaction.Rollback();
                    }
                }
                catch (Exception exception)
                {
                    transaction.Rollback();
                    Logger.Error(exception.Message, exception);

                    throw;
                }
            }

            // Notification
            if (null != transferBundle)
            {
                var eventBroker = Container.Resolve<IEventBroker>();

                eventBroker.Notify(new TransferBundleNotification(transferBundle));

                foreach (var updatedTransfer in updatedTransfers)
                {
                    eventBroker.Notify(new PreparedTransferNotification(updatedTransfer));
                }
            }
        }

        public IPreparedTransfer TryObtainTransferForProcessing()
        {
            TransferBundle transferBundle;

            using (var context = new DataContext(Session.AuthenticationService.GetConnectionSettings()))
            {
                transferBundle = (from tb in context.TransferBundles
                    where tb.State == TransferBundleState.Processed
                    select tb).FirstOrDefault();

                if (null == transferBundle)
                {
                    transferBundle = (from tb in context.TransferBundles
                        where tb.State == TransferBundleState.Pended
                        select tb).FirstOrDefault();
                }
            }

            if (null == transferBundle)
                return null;

            PreparedTransfer preparedTransfer;
            int bundleId = transferBundle.Id;

            using (var context = new DataContext(Session.AuthenticationService.GetConnectionSettings()))
            {
                var transaction = context.Database.BeginTransaction(IsolationLevel.ReadCommitted);

                try
                {
                    LockBundle(context, bundleId);

                    transferBundle = (from tb in context.TransferBundles
                        where tb.Id == bundleId
                        select tb).First();

                    if (transferBundle.State == TransferBundleState.Pended)
                    {
                        transferBundle.State = TransferBundleState.Processed;
                        transferBundle.UpdateTime = DateTime.UtcNow;
                    }

                    if (transferBundle.State != TransferBundleState.Processed)
                    {
                        transaction.Rollback();
                        return null;
                    }

                    preparedTransfer = (from pt in context.PreparedTransfers
                        where pt.TransferBundleId == bundleId
                              && pt.State == PreparedTransferState.Pended
                        select pt).FirstOrDefault();

                    if (null == preparedTransfer)
                    {
                        transferBundle.State = TransferBundleState.Completed;
                        transferBundle.UpdateTime = DateTime.UtcNow;
                    }
                    else
                    {
                        transferBundle.PendedCount--;
                        transferBundle.PendedTotalAmount -= preparedTransfer.Amount;

                        transferBundle.ProcessedCount++;
                        transferBundle.ProcessedTotalAmount += preparedTransfer.Amount;

                        preparedTransfer.State = PreparedTransferState.Processed;
                        preparedTransfer.UpdateTime = DateTime.UtcNow;
                    }

                    context.SaveChanges();
                    transaction.Commit();

                }
                catch (Exception exception)
                {
                    transaction.Rollback();
                    Logger.Error(exception.Message, exception);

                    throw;
                }
            }

            // Notification
            var eventBroker = Container.Resolve<IEventBroker>();

            eventBroker.Notify(new TransferBundleNotification(transferBundle));

            if (null != preparedTransfer)
                eventBroker.Notify(new PreparedTransferNotification(preparedTransfer));

            return preparedTransfer;
        }

        public void ProcessPreparedTransfer(int preparedTransferId)
        {
            PreparedTransfer preparedTransfer;

            using (var context = new DataContext(Session.AuthenticationService.GetConnectionSettings()))
            {
                preparedTransfer =
                    context.PreparedTransfers.First(pt => pt.Id == preparedTransferId);
            }

            if (preparedTransfer.State != PreparedTransferState.Processed)
                return;

            var transferBundleId = preparedTransfer.TransferBundleId;

            var transferService = Container.Resolve<IExternalTransferService>();

            PreparedTransferState preparedTransferState;
            string errrorMessage = null;

            try
            {
                transferService.CreateTransfer(new OriginalTransfer(preparedTransfer.PaymentId,
                    preparedTransfer.SourcePurse, preparedTransfer.TargetPurse, preparedTransfer.Amount,
                    preparedTransfer.Description));


                preparedTransferState = PreparedTransferState.Completed;
            }
            catch (Exception exception)
            {
                Logger.Error(exception.Message, exception);
                errrorMessage = exception.Message;
                preparedTransferState = PreparedTransferState.Failed;
            }

            TransferBundle transferBundle;

            using (var context = new DataContext(Session.AuthenticationService.GetConnectionSettings()))
            {
                var transaction = context.Database.BeginTransaction(IsolationLevel.ReadCommitted);

                try
                {
                    LockBundle(context, transferBundleId);

                    preparedTransfer = context.PreparedTransfers.Include("TransferBundle")
                        .First(pt => pt.Id == preparedTransferId);

                    // TODO [M] Получить данные платежа.
                    preparedTransfer.State = preparedTransferState;
                    preparedTransfer.ErrorMessage = errrorMessage;

                    transferBundle = preparedTransfer.TransferBundle;

                    transferBundle.ProcessedCount--;
                    transferBundle.ProcessedTotalAmount -= preparedTransfer.Amount;

                    switch (preparedTransferState)
                    {
                        case PreparedTransferState.Failed:
                            transferBundle.FailedCount++;
                            transferBundle.FailedTotalAmount += preparedTransfer.Amount;
                            break;
                        case PreparedTransferState.Completed:
                            transferBundle.CompletedCount++;
                            transferBundle.CompletedTotalAmount += preparedTransfer.Amount;
                            break;
                        default:
                            throw new InvalidOperationException("treparedTransferState == " + preparedTransferState);
                    }

                    context.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception exception)
                {
                    transaction.Rollback();
                    Logger.Error(exception.Message, exception);

                    throw;
                }
            }

            var eventBroker = Container.Resolve<IEventBroker>();

            eventBroker.Notify(new PreparedTransferNotification(preparedTransfer));
            eventBroker.Notify(new TransferBundleNotification(transferBundle));
        }

        private List<PreparedTransfer> Start(TransferBundle transferBundle)
        {
            if (TransferBundleState.Registered != transferBundle.State)
                throw new InvalidOperationException("transferBundle.State == " + transferBundle.State);

            transferBundle.State = TransferBundleState.Pended;
            transferBundle.UpdateTime = DateTime.UtcNow;

            var updatedTransfers = new List<PreparedTransfer>();

            foreach (var transfer in transferBundle.Transfers)
            {
                if (PreparedTransferState.Registered != transfer.State)
                    continue;

                transfer.State = PreparedTransferState.Pended;
                transfer.UpdateTime = DateTime.UtcNow;

                transferBundle.RegisteredCount--;
                transferBundle.RegisteredTotalAmount -= transfer.Amount;

                transferBundle.PendedCount++;
                transferBundle.PendedTotalAmount += transfer.Amount;

                updatedTransfers.Add(transfer);
            }

            return updatedTransfers;
        }

        private List<PreparedTransfer> Stop(TransferBundle transferBundle)
        {
            if (TransferBundleState.Pended != transferBundle.State &&
                TransferBundleState.Processed != transferBundle.State)
                throw new InvalidOperationException("transferBundle.State == " + transferBundle.State);

            transferBundle.State = TransferBundleState.Registered;
            transferBundle.UpdateTime = DateTime.UtcNow;

            var updatedTransfers = new List<PreparedTransfer>();

            foreach (var transfer in transferBundle.Transfers)
            {
                if (PreparedTransferState.Pended != transfer.State)
                    continue;

                transfer.State = PreparedTransferState.Registered;
                transfer.UpdateTime = DateTime.UtcNow;

                transferBundle.PendedCount--;
                transferBundle.PendedTotalAmount -= transfer.Amount;

                transferBundle.RegisteredCount++;
                transferBundle.RegisteredTotalAmount += transfer.Amount;

                updatedTransfers.Add(transfer);
            }

            return updatedTransfers;
        }

        private static void LockBundle(DataContext dataContext, int bundleId)
        {
            const string sql = "UPDATE TransferBundle " +
                               "SET UpdateTime = {0} " +
                               "WHERE Id = {1}";

            dataContext.Database.ExecuteSqlCommand(sql, DateTime.UtcNow, bundleId);

            var bundle = (from entity in dataContext.TransferBundles
                where entity.Id == bundleId
                select entity).First();

            dataContext.Entry(bundle).Reload();
        }
    }
}
