using System;
using WebMoney.Services.Contracts.BusinessObjects;

namespace WMBusinessTools.Extensions.BusinessObjects
{
    internal sealed class TrustConfirmation : ITrustConfirmation
    {
        public int Reference { get; }
        public string ConfirmationCode { get; }

        public TrustConfirmation(int reference, string confirmationCode)
        {
            Reference = reference;
            ConfirmationCode = confirmationCode ?? throw new ArgumentNullException(nameof(confirmationCode));
        }
    }
}
