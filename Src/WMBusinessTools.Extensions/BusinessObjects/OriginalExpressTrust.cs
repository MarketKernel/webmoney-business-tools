using System;
using WebMoney.Services.Contracts.BasicTypes;
using WebMoney.Services.Contracts.BusinessObjects;

namespace WMBusinessTools.Extensions.BusinessObjects
{
    internal sealed class OriginalExpressTrust : IOriginalExpressTrust
    {
        public string StorePurse { get; }
        public IExtendedIdentifier Identifier { get; }

        public decimal DayLimit { get; set; }
        public decimal WeekLimit { get; set; }
        public decimal MonthLimit { get; set; }

        public ConfirmationType ConfirmationType { get; set; }

        public OriginalExpressTrust(string storePurse, ExtendedIdentifier identifier)
        {
            StorePurse = storePurse ?? throw new ArgumentNullException(nameof(storePurse));
            Identifier = identifier ?? throw new ArgumentNullException(nameof(identifier));
        }
    }
}
