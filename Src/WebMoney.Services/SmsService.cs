using System;
using WebMoney.Services.Contracts;
using WebMoney.Services.Contracts.Exceptions;
using WebMoney.Services.SmsServiceReference;
using WebMoney.XmlInterfaces.BasicObjects;

namespace WebMoney.Services
{
    public sealed class SmsService : SessionBasedService, ISmsService
    {
        public void SendSms(string payFromPurse, string phoneNumber, string message, bool transliterate)
        {
            if (null == phoneNumber)
                throw new ArgumentNullException(nameof(phoneNumber));

            if (null == message)
                throw new ArgumentNullException(nameof(message));

            phoneNumber = "+" + phoneNumber;

            var masterIdentifierValue = ((WmId)Session.AuthenticationService.MasterIdentifier).ToString();
            string dateTimeValue = DateTime.Now.ToString(@"yyyy-MM-dd HH\:mm\:ss");
            string sign =
                Session.AuthenticationService.Sign(phoneNumber + message +
                                                   (WmId) Session.AuthenticationService.MasterIdentifier +
                                                   (payFromPurse ?? string.Empty) + dateTimeValue);

            int smsId;

            var soapClient = new SmsSenderSvcSoapClient();
            int result = soapClient.SendSingleSMS(phoneNumber, message, transliterate, masterIdentifierValue,
                payFromPurse ?? string.Empty, dateTimeValue, null == payFromPurse, sign, out smsId);

            // TODO [L] Exception SmsService
            switch (result)
            {
                case 0:
                    return;
                case 1:
                    throw new ExternalException("Внутренняя ошибка сервиса sms.webmoney.ru");
                case 2:
                    throw new ExternalException("Неверный параметр dateTime (более, чем на сутки отличается от времени на сервере)");
                case 3:
                    throw new ExternalException("Неверная подпись запроса");
                case 4:
                    throw new ExternalException(
                        "Невозможно осуществить оплату (недостаточно средств или неверный кошелек). Пополните ваш баланс на sms.webmoney.ru");
                default:
                    throw new ExternalException(
                        "Другая неизвестная ошибка sms.webmoney.ru. Номер ошибки " + result + ". Обратитесь к администратору sms.webmoney.ru.");
            }
        }
    }
}
