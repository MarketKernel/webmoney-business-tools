using System;
using WebMoney.Services.Contracts.BasicTypes;
using WebMoney.Services.Contracts.BusinessObjects;

namespace WebMoney.Services.BusinessObjects
{
    internal sealed class TrustConfirmationReport : ITrustConfirmationReport
    {
        public IExpressTrust Trust { get; }
        public string PublicMessage { get; }
        public SmsState SmsState { get; }

        public TrustConfirmationReport(ExpressTrust trust, string publicMessage, SmsState smsState)
        {
            Trust = trust ?? throw new ArgumentNullException(nameof(trust));
            PublicMessage = publicMessage ?? throw new ArgumentNullException(nameof(publicMessage));
            SmsState = smsState;
        }
    }
}
