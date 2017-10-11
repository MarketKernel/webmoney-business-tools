using System;
using System.Linq;
using System.Net.Mail;
using WebMoney.Services.BusinessObjects;
using WebMoney.Services.Contracts;
using WebMoney.Services.Contracts.BasicTypes;
using WebMoney.Services.Contracts.BusinessObjects;
using WebMoney.Services.Contracts.Exceptions;
using WebMoney.Services.DataAccess.EF;
using WebMoney.Services.Utils;
using WebMoney.XmlInterfaces;
using WebMoney.XmlInterfaces.BasicObjects;
using WebMoney.XmlInterfaces.Exceptions;
using WebMoney.XmlInterfaces.Responses;

namespace WebMoney.Services
{
    public sealed class PaymentService : SessionBasedService, IPaymentService
    {
        public IPaymentConfirmationInstruction RequestPayment(IOriginalExpressPayment originalExpressPayment)
        {
            if (null == originalExpressPayment)
                throw new ArgumentNullException(nameof(originalExpressPayment));

            ExpressPaymentRequest request;

            switch (originalExpressPayment.ExtendedIdentifier.Type)
            {
                case ExtendedIdentifierType.Phone:
                    var phone = originalExpressPayment.ExtendedIdentifier.Value;

                    if (!phone.StartsWith("+"))
                        phone = $"+{phone}";

                    request = new ExpressPaymentRequest(Purse.Parse(originalExpressPayment.TargetPurse),
                        (uint) originalExpressPayment.OrderId, (Amount) originalExpressPayment.Amount,
                        (Description) originalExpressPayment.Description,
                        Phone.Parse(phone),
                        ConvertFrom.ContractTypeToApiType(originalExpressPayment.ConfirmationType));
                    break;
                case ExtendedIdentifierType.WmId:
                    request = new ExpressPaymentRequest(Purse.Parse(originalExpressPayment.TargetPurse),
                        (uint) originalExpressPayment.OrderId, (Amount) originalExpressPayment.Amount,
                        (Description) originalExpressPayment.Description,
                        WmId.Parse(originalExpressPayment.ExtendedIdentifier.Value),
                        ConvertFrom.ContractTypeToApiType(originalExpressPayment.ConfirmationType));
                    break;
                case ExtendedIdentifierType.Email:
                    request = new ExpressPaymentRequest(Purse.Parse(originalExpressPayment.TargetPurse),
                        (uint) originalExpressPayment.OrderId, (Amount) originalExpressPayment.Amount,
                        (Description) originalExpressPayment.Description,
                        new MailAddress(originalExpressPayment.ExtendedIdentifier.Value),
                        ConvertFrom.ContractTypeToApiType(originalExpressPayment.ConfirmationType));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            request.Initializer = CreateInitializer(originalExpressPayment.TargetPurse);

            ExpressPaymentResponse response;

            try
            {
                response = request.Submit();
            }
            catch (WmException exception)
            {
                throw new ExternalServiceException(exception.Message, exception);
            }

            return new PaymentConfirmationInstruction(response.InvoiceId,
                ConvertFrom.ApiTypeToContractType(response.ConfirmationType), response.Info);
        }

        public IExpressPayment ConfirmPayment(IPaymentConfirmation confirmation)
        {
            if (null == confirmation)
                throw new ArgumentNullException(nameof(confirmation));

            var request = new ExpressPaymentConfirmation(Purse.Parse(confirmation.TargetPurse),
                confirmation.ConfirmationCode, (uint) confirmation.InvoiceId, null)
            {
                Initializer = CreateInitializer(confirmation.TargetPurse)
        };

            ExpressPaymentReport response;

            try
            {
                response = request.Submit();
            }
            catch (WmException exception)
            {
                throw new ExternalServiceException(exception.Message, exception);
            }

            var expressPayment = new ExpressPayment((int) response.TransferId, (int) response.InvoiceId,
                response.Amount, response.Date.ToUniversalTime(), response.ClientPurse.ToString(), response.ClientWmId);

            return expressPayment;
        }

        public string CreatePaymentLink(IPaymentLinkRequest paymentLinkRequest)
        {
            if (null == paymentLinkRequest)
                throw new ArgumentNullException(nameof(paymentLinkRequest));

            var request = new OriginalMerchantPayment((uint) paymentLinkRequest.OrderId,
                Purse.Parse(paymentLinkRequest.StorePurse), (Amount) paymentLinkRequest.Amount,
                (Description) paymentLinkRequest.Description, (uint) paymentLinkRequest.Lifetime)
            {
                Initializer = CreateInitializer(paymentLinkRequest.StorePurse)
            };

            MerchantPaymentToken response;

            try
            {
                response = request.Submit();
            }
            catch (WmException exception)
            {
                throw new ExternalServiceException(exception.Message, exception);
            }

            return response.Token;
        }

        public IMerchantPayment FindPayment(string purse, long transferPrimaryId,
            Contracts.BasicTypes.PaymentNumberKind numberKind)
        {
            if (null == purse)
                throw new ArgumentNullException(nameof(purse));

            var request = new MerchantOperationObtainer(Purse.Parse(purse), (uint) transferPrimaryId,
                ConvertFrom.ContractTypeToApiType(numberKind))
            {
                Initializer = CreateInitializer(purse)
            };

            MerchantOperation response;

            try
            {
                response = request.Submit();
            }
            catch (WmException exception)
            {
                throw new ExternalServiceException(exception.Message, exception);
            }

            var merchantPayment = new MerchantPayment(response.OperationId, response.InvoiceId,
                response.Amount, response.CreateTime.ToUniversalTime(), response.Description, response.SourceWmId,
                response.SourcePurse.ToString())
            {
                IsCapitaller = response.CapitallerFlag,
                IsEnum = response.EnumFlag,
                IPAddress = response.IpAddress,
                TelepatPhone = response.TelepatPhone,
                PaymerNumber = response.PaymerNumber,
                PaymerEmail = response.PaymerEmail,
                CashierNumber = response.CashierNumber,
                CashierDate = response.CashierDate?.ToUniversalTime(),
                CashierAmount = response.CashierAmount,
            };


            if (null != response.TelepatMethod)
                merchantPayment.TelepatMethod = ConvertFrom.ApiTypeToContractType(response.TelepatMethod.Value);

            if (XmlInterfaces.BasicObjects.PaymerType.None != response.PaymerType)
                merchantPayment.PaymerType = ConvertFrom.ApiTypeToContractType(response.PaymerType);

            // TODO [L] Расшифровать тип e-invoicing платежа.
            if (null != response.SdpType)
                merchantPayment.InvoicingMethod = (byte) response.SdpType.Value;

            return merchantPayment;
        }

        private Initializer CreateInitializer(string storePurse)
        {
            string merchantKey = null;

            if (Session.AuthenticationService.HasConnectionSettings)
            {
                using (var context = new DataContext(Session.AuthenticationService.GetConnectionSettings()))
                {
                    merchantKey = (from a in context.Accounts
                        where a.Number == storePurse
                        select a.MerchantKey).FirstOrDefault();
                }
            }

            if (null == merchantKey && AuthenticationMethod.KeeperClassic !=
                Session.AuthenticationService.AuthenticationMethod)
                throw new InvalidOperationException(
                    "null == merchantKey && AuthenticationMethod.KeeperClassic != Session.AuthenticationService.AuthenticationMethod");

            var initializer = null != merchantKey
                ? new Initializer((WmId) Session.CurrentIdentifier, merchantKey)
                : Session.AuthenticationService.ObtainInitializer();

            return initializer;
        }
    }
}