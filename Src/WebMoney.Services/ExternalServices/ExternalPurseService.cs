using System.Collections.Generic;
using System.Linq;
using log4net;
using WebMoney.Services.BusinessObjects;
using WebMoney.Services.Contracts.BusinessObjects;
using WebMoney.Services.Contracts.Exceptions;
using WebMoney.Services.ExternalServices.Contracts;
using WebMoney.Services.Utils;
using WebMoney.XmlInterfaces;
using WebMoney.XmlInterfaces.BasicObjects;
using WebMoney.XmlInterfaces.Exceptions;
using WebMoney.XmlInterfaces.Responses;

namespace WebMoney.Services.ExternalServices
{
    internal sealed class ExternalPurseService : SessionBasedService, IExternalPurseService
    {
        private const string EmptyPurseName = "[empty]";
        private const string UnknownPurseName = "?";

        private static readonly ILog Logger = LogManager.GetLogger(typeof(ExternalPurseService));

        public IReadOnlyCollection<IAccount> SelectAccounts()
        {
            long currentIdentifier = Session.CurrentIdentifier;

            if (Session.IsMaster())
            {
                var request = new PurseInfoFilter((WmId) currentIdentifier)
                {
                    Initializer = Session.AuthenticationService.ObtainInitializer()
                };

                PurseInfoRegister response;

                try
                {
                    response = request.Submit();
                }
                catch (WmException exception)
                {
                    throw new ExternalException(exception.Message, exception);
                }

                return response.PurseInfoList.Select(
                        pi =>
                        {
                            string description = pi.Description;

                            if (string.IsNullOrWhiteSpace(pi.Description))
                                description = EmptyPurseName;

                            var account =
                                new Account(pi.Purse.ToString(), currentIdentifier,
                                    description) {Amount = pi.Amount};

                            if (pi.LastIncomingTransfer > 0)
                                account.LastIncomingTransferPrimaryId = pi.LastIncomingTransfer;

                            if (pi.LastOutgoingTransfer > 0)
                                account.LastOutgoingTransferPrimaryId = pi.LastOutgoingTransfer;

                            return (IAccount) account;
                        })
                    .ToList();
            }

            var trustFilter = new IncomingTrustFilter((WmId) currentIdentifier)
            {
                Initializer = Session.AuthenticationService.ObtainInitializer()
            };

            var purseInfoFilter = new PurseInfoFilter((WmId) currentIdentifier)
            {
                Initializer = Session.AuthenticationService.ObtainInitializer()
            };

            TrustRegister trustRegister;

            try
            {
                trustRegister = trustFilter.Submit();
            }
            catch (WmException exception)
            {
                throw new ExternalException(exception.Message, exception);
            }

            PurseInfoRegister purseInfoRegister;

            try
            {
                purseInfoRegister = purseInfoFilter.Submit();
            }
            catch (WmException exception)
            {
                Logger.Warn(exception.Message, exception);
                purseInfoRegister = null;
            }

            var accounts = new List<IAccount>();

            foreach (var trust in trustRegister.TrustList)
            {
                if (!trust.Purse.HasValue)
                    continue;

                string purseNumber = trust.Purse.Value.ToString();

                var purseInfo =
                    purseInfoRegister?.PurseInfoList.FirstOrDefault(pi => pi.Purse == trust.Purse.Value);

                string description;

                if (null == purseInfo)
                    description = UnknownPurseName;
                else
                {
                    description = purseInfo.Description;

                    if (string.IsNullOrWhiteSpace(description))
                        description = EmptyPurseName;
                }

                var account = new Account(purseNumber, currentIdentifier, description);

                if (null != purseInfo)
                {
                    account.Amount = purseInfo.Amount;

                    if (purseInfo.LastIncomingTransfer > 0)
                        account.LastIncomingTransferPrimaryId = purseInfo.LastIncomingTransfer;

                    if (purseInfo.LastOutgoingTransfer > 0)
                        account.LastOutgoingTransferPrimaryId = purseInfo.LastOutgoingTransfer;
                }

                account.InvoiceAllowed = trust.InvoiceAllowed;
                account.TransferAllowed = trust.TransferAllowed;
                account.BalanceAllowed = trust.BalanceAllowed;
                account.HistoryAllowed = trust.HistoryAllowed;

                account.DayLimit = trust.DayLimit;
                account.WeekLimit = trust.WeekLimit;
                account.MonthLimit = trust.MonthLimit;

                account.DayTotalAmount = trust.DayAmount;
                account.WeekTotalAmount = trust.WeekAmount;
                account.MonthTotalAmount = trust.MonthAmount;

                account.LastTransferTime = trust.LastDate.ToUniversalTime();

                accounts.Add(account);
            }

            return accounts;
        }
    }
}
