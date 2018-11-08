using System;
using System.Collections.Generic;
using System.Linq;
using Unity;
using WebMoney.Services.BusinessObjects;
using WebMoney.Services.Contracts;
using WebMoney.Services.Contracts.BusinessObjects;
using WebMoney.Services.Contracts.Exceptions;
using WebMoney.Services.DataAccess.EF;
using WebMoney.Services.ExternalServices.Contracts;
using WebMoney.Services.Utils;
using WebMoney.XmlInterfaces;
using WebMoney.XmlInterfaces.BasicObjects;
using WebMoney.XmlInterfaces.Exceptions;

namespace WebMoney.Services
{
    public sealed class TransferService : SessionBasedService, ITransferService
    {
        private const decimal MinimalCommission = 0.01M;

        public long CreateTransfer(IOriginalTransfer originalTransfer)
        {
            if (null == originalTransfer)
                throw new ArgumentNullException(nameof(originalTransfer));

            var externalTransferService = Container.Resolve<IExternalTransferService>();
            return externalTransferService.CreateTransfer(originalTransfer);
        }

        public void Moneyback(long transferPrimaryId, decimal amount)
        {
            var request =
                new TransferRejector(transferPrimaryId, (Amount) amount)
                {
                    Initializer = Session.AuthenticationService.ObtainInitializer()
                };

            try
            {
                request.Submit();
            }
            catch (WmException exception)
            {
                throw new ExternalServiceException(exception.Message, exception);
            }
        }

        public void FinishProtection(long transferPrimaryId, string protectionCode)
        {
            var request = new ProtectionFinisher(transferPrimaryId, (Description) protectionCode)
            {
                Initializer = Session.AuthenticationService.ObtainInitializer()
            };

            try
            {
                request.Submit();
            }
            catch (WmException exception)
            {
                throw new ExternalServiceException(exception.Message, exception);
            }
        }

        public void RejectProtection(long transferPrimaryId)
        {
            var request =
                new ProtectionRejector(transferPrimaryId)
                {
                    Initializer = Session.AuthenticationService.ObtainInitializer()
                };

            try
            {
                request.Submit();
            }
            catch (WmException exception)
            {
                throw new ExternalServiceException(exception.Message, exception);
            }
        }

        public void RedeemPaymer(string purse, string number, string key)
        {
            if (null == purse)
                throw new ArgumentNullException(nameof(purse));

            if (null == number)
                throw new ArgumentNullException(nameof(number));

            if (null == key)
                throw new ArgumentNullException(nameof(key));

            var request =
                new PaymerTransfer(Purse.Parse(purse), (Description) number, (Description) key)
                {
                    Initializer = Session.AuthenticationService.ObtainInitializer()
                };

            try
            {
                request.Submit();
            }
            catch (WmException exception)
            {
                throw new ExternalServiceException(exception.Message, exception);
            }
        }

        public IEnumerable<ITransfer> SelectTransfers(string purse, DateTime fromTime, DateTime toTime,
            bool fresh = false)
        {
            if (null == purse)
                throw new ArgumentNullException(nameof(purse));

            List<Transfer> transfers;

            long currentIdentifier = Session.CurrentIdentifier;

            if (!fresh && Session.AuthenticationService.HasConnectionSettings)
            {
                using (var context = new DataContext(Session.AuthenticationService.GetConnectionSettings()))
                {
                    transfers = context.Transfers.Select(currentIdentifier, purse, fromTime, toTime);
                }
            }
            else
            {
                // TODO [L] Применить копирование из ITransfer в Transfer (тип может не совпадать!).
                var externalTransferService = Container.Resolve<IExternalTransferService>();
                transfers = externalTransferService.SelectTransfers(purse, fromTime, toTime)
                    .Select(t => (Transfer) t)
                    .ToList();

                if (Session.AuthenticationService.HasConnectionSettings)
                    UpdateLocalTransfers(transfers);
            }

            foreach (var transfer in transfers)
            {
                if (transfer.SourcePurse.Equals(purse))
                    transfer.OutcomeAmount = transfer.Amount;
                else
                    transfer.IncomeAmount = transfer.Amount;
            }

            return transfers.OrderByDescending(t => t.UpdateTime);
        }

        public decimal CalculateCommission(decimal amount, string currency)
        {
            if (amount < 0.01M)
                throw new ArgumentOutOfRangeException(nameof(amount));

            if (null == currency)
                throw new ArgumentNullException(nameof(currency));

            var commission = amount * 0.008M;

            if (commission < MinimalCommission)
                commission = MinimalCommission;

            commission *= 100;
            commission = Math.Ceiling(commission);
            commission /= 100;

            decimal maximalCommission = GetMaximalCommission(currency);

            if (commission > maximalCommission)
                commission = maximalCommission;

            return commission;
        }

        private decimal GetMaximalCommission(string currency)
        {
            decimal maximalCommission = decimal.MaxValue;

            switch (currency.ToUpper())
            {
                case "Z":
                    maximalCommission = 50M;
                    break;
                case "E":
                    maximalCommission = 50M;
                    break;
                case "R":
                    maximalCommission = 1500M;
                    break;
                case "U":
                    maximalCommission = 250M;
                    break;
                case "B":
                    maximalCommission = 100M;
                    break;
                case "K":
                    maximalCommission = 9000M;
                    break;
                case "V":
                    maximalCommission = 1000000M;
                    break;
                case "Y":
                    maximalCommission = 50000M;
                    break;
                case "G":
                    maximalCommission = 2M;
                    break;
                case "X":
                    maximalCommission = 10M;
                    break;
                case "H":
                    maximalCommission = 100M;
                    break;
            }

            return maximalCommission;
        }

        private void UpdateLocalTransfers(List<Transfer> externalTransfers)
        {
            long currentIdentifier = Session.CurrentIdentifier;

            using (var context = new DataContext(Session.AuthenticationService.GetConnectionSettings()))
            {
                var outdatedTransfers = new List<Transfer>();
                var recentTransfers = new List<Transfer>();

                foreach (var externalTransfer in externalTransfers)
                {
                    var primaryId = externalTransfer.PrimaryId;

                    var localSecondaryId = (from t in context.Transfers
                        where t.Identifier == currentIdentifier && t.PrimaryId == primaryId
                        select t.SecondaryId).FirstOrDefault();

                    if (0 == localSecondaryId)
                    {
                        recentTransfers.Add(externalTransfer);
                        continue;
                    }

                    if (externalTransfer.SecondaryId != localSecondaryId)
                    {
                        var localTransfer =
                            context.Transfers.FirstOrDefault(
                                t => t.Identifier == currentIdentifier && t.PrimaryId == primaryId);

                        outdatedTransfers.Add(localTransfer);
                        recentTransfers.Add(externalTransfer);
                    }
                }

                context.Transfers.RemoveRange(outdatedTransfers);
                context.Transfers.AddRange(recentTransfers);

                context.SaveChanges();
            }
        }
    }
}