using System;
using System.Collections.Generic;

namespace WMBusinessTools.Extensions.StronglyTypedWrappers
{
    internal sealed class CreateTransferFormValuesWrapper : StronglyTypedValuesWrapper
    {
        public const string Control2ToPurseCommandFindIdentifier = "FindIdentifier";

        public int Control1TransferId
        {
            get => (int)(decimal)GetValue(0);
            set => SetValue("TransferId", (decimal)value);
        }
        public string Control2ToPurse => (string) GetValue(1);
        public string Control3FromPurse => (string) GetValue(2);

        public decimal Control4Amount
        {
            get => (decimal) GetValue(3);
            set => SetValue("Amount", value);
        }

        public string Control5Description => (string) GetValue(4);
        public bool Control6UsePaymentProtection => (bool) GetValue(5);

        public byte Control7ProtectionPeriod
        {
            get => (byte)(decimal) GetValue(6);
            set => SetValue("ProtectionPeriod", (decimal) value);
        }

        public string Control8ProtectionCode => (string) GetValue(7);

        public CreateTransferFormValuesWrapper()
        {
        }

        public CreateTransferFormValuesWrapper(List<object> values)
        {
            if (null == values)
                throw new ArgumentNullException(nameof(values));

            ApplyOutcomeValues(values);
        }
    }
}