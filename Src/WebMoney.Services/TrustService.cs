using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using Unity;
using WebMoney.Services.BusinessObjects;
using WebMoney.Services.Contracts;
using WebMoney.Services.Contracts.BasicTypes;
using WebMoney.Services.Contracts.BusinessObjects;
using WebMoney.Services.Contracts.Exceptions;
using WebMoney.Services.DataAccess.EF;
using WebMoney.Services.ExternalServices.Contracts;
using WebMoney.Services.Utils;
using WebMoney.XmlInterfaces;
using WebMoney.XmlInterfaces.BasicObjects;
using WebMoney.XmlInterfaces.Exceptions;
using WebMoney.XmlInterfaces.Responses;
using Trust = WebMoney.Services.BusinessObjects.Trust;

namespace WebMoney.Services
{
    public sealed class TrustService : SessionBasedService, ITrustService
    {
        public void CreateTrust(IOriginalTrust trust)
        {
            if (null == trust)
                throw new ArgumentNullException(nameof(trust));

            var request =
                new OriginalTrust((WmId) trust.MasterIdentifier, Purse.Parse(trust.Purse))
                {
                    InvoiceAllowed = trust.InvoiceAllowed,
                    TransferAllowed = trust.TransferAllowed,
                    BalanceAllowed = trust.BalanceAllowed,
                    HistoryAllowed = trust.HistoryAllowed,
                    DayLimit = (Amount) trust.DayLimit,
                    WeekLimit = (Amount) trust.WeekLimit,
                    MonthLimit = (Amount) trust.MonthLimit,
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

        public ITrustConfirmationInstruction RequestTrust(IOriginalExpressTrust originalExpressTrust)
        {
            if (null == originalExpressTrust)
                throw new ArgumentNullException(nameof(originalExpressTrust));

            XmlInterfaces.BasicObjects.ConfirmationType confirmationType =
                ConvertFrom.ContractTypeToApiType(originalExpressTrust.ConfirmationType);

            ExpressTrustRequest request;

            switch (originalExpressTrust.Identifier.Type)
            {
                case ExtendedIdentifierType.Phone:
                    var phone = originalExpressTrust.Identifier.Value;

                    if (!phone.StartsWith("+"))
                        phone = $"+{phone}";

                    request = new ExpressTrustRequest(Purse.Parse(originalExpressTrust.StorePurse),
                        (Amount)originalExpressTrust.DayLimit, (Amount)originalExpressTrust.WeekLimit,
                        (Amount)originalExpressTrust.MonthLimit, Phone.Parse(phone),
                        confirmationType);
                    break;
                case ExtendedIdentifierType.WmId:
                    request = new ExpressTrustRequest(Purse.Parse(originalExpressTrust.StorePurse),
                        (Amount)originalExpressTrust.DayLimit, (Amount)originalExpressTrust.WeekLimit,
                        (Amount)originalExpressTrust.MonthLimit, WmId.Parse(originalExpressTrust.Identifier.Value),
                        confirmationType);
                    break;
                case ExtendedIdentifierType.Email:
                    MailAddress mailAddress = new MailAddress(originalExpressTrust.Identifier.Value);

                    request = new ExpressTrustRequest(Purse.Parse(originalExpressTrust.StorePurse),
                        (Amount)originalExpressTrust.DayLimit, (Amount)originalExpressTrust.WeekLimit,
                        (Amount)originalExpressTrust.MonthLimit, mailAddress,
                        confirmationType);
                    break;
                case ExtendedIdentifierType.Purse:
                    request = new ExpressTrustRequest(Purse.Parse(originalExpressTrust.StorePurse),
                        (Amount)originalExpressTrust.DayLimit, (Amount)originalExpressTrust.WeekLimit,
                        (Amount)originalExpressTrust.MonthLimit, Purse.Parse(originalExpressTrust.Identifier.Value),
                        confirmationType);
                    break;
                default:
                    throw new InvalidOperationException("originalExpressTrust.Identifier.Type == " +
                                                        originalExpressTrust.Identifier.Type);
            }

            request.DayLimit = (Amount)originalExpressTrust.DayLimit;
            request.WeekLimit = (Amount)originalExpressTrust.WeekLimit;
            request.MonthLimit = (Amount) originalExpressTrust.MonthLimit;

            request.Initializer = Session.AuthenticationService.ObtainInitializer();

            ExpressTrustResponse response;

            try
            {
                response = request.Submit();
            }
            catch (WmException exception)
            {
                throw new ExternalServiceException(exception.Message, exception);
            }

            var instruction = new TrustConfirmationInstruction(response.Reference,
                ConvertFrom.ApiTypeToContractType(response.ConfirmationType), response.Info, response.SmsReference);

            return instruction;
        }

        public IExpressTrust ConfirmTrust(ITrustConfirmation trustConfirmation)
        {
            if (null == trustConfirmation)
                throw new ArgumentNullException(nameof(trustConfirmation));

            var request = new ExpressTrustConfirmation(trustConfirmation.Reference,
                trustConfirmation.ConfirmationCode)
            {
                Initializer = Session.AuthenticationService.ObtainInitializer()
            };

            ExpressTrustReport response ;

            try
            {
                response = request.Submit();
            }
            catch (WmException exception)
            {
                throw new ExternalServiceException(exception.Message, exception);
            }

            var expressTrust = new ExpressTrust(response.TrustId,
                response.ClientPurse.ToString(), response.ClientWmId, response.Info);

            return expressTrust;
        }

        public IEnumerable<ITrust> SelectTrusts(bool fresh = false)
        {
            List<Trust> trusts;

            if (!fresh)
            {
                if (Session.AuthenticationService.HasConnectionSettings)
                {
                    using (var context = new DataContext(Session.AuthenticationService.GetConnectionSettings()))
                    {
                        trusts = context.Trusts.ToList();
                    }
                }
                else
                    trusts = new List<Trust>();
            }
            else
            {
                // TODO [L] Применить копирование из ITrust в Trust (тип может не совпадать!).
                var externalTrustService = Container.Resolve<IExternalTrustService>();
                trusts = externalTrustService.SelectTrusts().Select(t => (Trust) t).ToList();

                if (Session.AuthenticationService.HasConnectionSettings)
                {
                    var entities = trusts.Select(t => t).ToList();

                    using (var context = new DataContext(Session.AuthenticationService.GetConnectionSettings()))
                    {
                        context.Trusts.RemoveRange(context.Trusts);
                        context.SaveChanges();

                        context.Trusts.AddRange(entities);
                        context.SaveChanges();
                    }
                }
            }

            return trusts;
        }
    }
}