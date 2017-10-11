using System;
using WebMoney.Services.BusinessObjects;
using WebMoney.Services.Contracts;
using WebMoney.Services.Contracts.BasicTypes;
using WebMoney.Services.Contracts.BusinessObjects;
using WebMoney.Services.Contracts.Exceptions;
using WebMoney.Services.Utils;
using WebMoney.XmlInterfaces;
using WebMoney.XmlInterfaces.BasicObjects;
using WebMoney.XmlInterfaces.Exceptions;
using WebMoney.XmlInterfaces.Responses;

namespace WebMoney.Services
{
    public sealed class VerificationService : SessionBasedService, IVerificationService
    {
        public IVerificationReport VerifyClient(ISuspectedClientInfo clientInfo)
        {
            if (null == clientInfo)
                throw new ArgumentNullException(nameof(clientInfo));

            var currency = Purse.LetterToCurrency(clientInfo.Currency[0]);
            var amount = (Amount) clientInfo.Amount;
            var wmId = (WmId) clientInfo.Identifier;

            ClientInspector request;

            switch (clientInfo.ExchangeType)
            {
                case Contracts.BasicTypes.ExchangeType.Cash:
                    request = new ClientInspector(currency, amount, wmId,
                        (Description) clientInfo.PassportNumber, (Description) clientInfo.SecondName,
                        (Description) clientInfo.FirstName);
                    break;
                case Contracts.BasicTypes.ExchangeType.OfflineSystem:
                    request = new ClientInspector(currency, amount, wmId, (Description) clientInfo.SecondName,
                        (Description) clientInfo.FirstName);
                    break;
                case Contracts.BasicTypes.ExchangeType.BankAccount:
                    request = new ClientInspector(currency, amount, wmId, (Description) clientInfo.SecondName,
                        (Description) clientInfo.FirstName, (Description) clientInfo.BankName,
                        BankAccount.Parse(clientInfo.BankAccount));
                    break;
                case Contracts.BasicTypes.ExchangeType.Bankcard:
                    request = new ClientInspector(currency, amount, wmId, (Description) clientInfo.SecondName,
                        (Description) clientInfo.FirstName, (Description) clientInfo.BankName,
                        BankCard.Parse(clientInfo.CardNumber));
                    break;
                case Contracts.BasicTypes.ExchangeType.InternetSystem:
                {
                    XmlInterfaces.BasicObjects.PaymentSystem paymentSystem;

                    switch (clientInfo.PaymentSystem)
                    {
                        case Contracts.BasicTypes.PaymentSystem.PayPal:
                            paymentSystem = XmlInterfaces.BasicObjects.PaymentSystem.PayPal;
                            break;
                        case Contracts.BasicTypes.PaymentSystem.Skrill:
                            paymentSystem = XmlInterfaces.BasicObjects.PaymentSystem.Skrill;
                            break;
                        case Contracts.BasicTypes.PaymentSystem.Alipay:
                            paymentSystem = XmlInterfaces.BasicObjects.PaymentSystem.Alipay;
                            break;
                        case Contracts.BasicTypes.PaymentSystem.Qiwi:
                            paymentSystem = XmlInterfaces.BasicObjects.PaymentSystem.Qiwi;
                            break;
                        case Contracts.BasicTypes.PaymentSystem.YandexMoney:
                            paymentSystem = XmlInterfaces.BasicObjects.PaymentSystem.YandexMoney;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    request = new ClientInspector(currency, amount, wmId, paymentSystem,
                        (Description) clientInfo.PaymentSystemClientId);
                }
                    break;
                case Contracts.BasicTypes.ExchangeType.Sms:
                    request = new ClientInspector(currency, amount, wmId, clientInfo.Phone);
                    break;
                case Contracts.BasicTypes.ExchangeType.Mobile:
                    request = new ClientInspector(currency, amount, wmId, clientInfo.Phone, true);
                    break;
                case Contracts.BasicTypes.ExchangeType.Blockchain:
                {
                    CryptoCurrencyType cryptoCurrencyType;

                    switch (clientInfo.CryptoCurrency)
                    {
                        case CryptoCurrency.Bitcoin:
                            cryptoCurrencyType = CryptoCurrencyType.Bitcoin;
                            break;
                        case CryptoCurrency.BitcoinCash:
                            cryptoCurrencyType = CryptoCurrencyType.BitcoinCash;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    request = new ClientInspector(currency, amount, wmId, cryptoCurrencyType,
                        clientInfo.CryptoCurrencyAddress);
                }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            request.Output = clientInfo.Output;
            request.Initializer = Session.AuthenticationService.ObtainInitializer();

            ClientEvidence response;

            try
            {
                response = request.Submit();
            }
            catch (WmException exception)
            {
                throw new ExternalServiceException(exception.Message, exception);
            }

            return new VerificationReport(response.Id, response.FirstName, response.Patronymic);
        }
    }
}
