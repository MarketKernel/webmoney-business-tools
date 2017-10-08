using System;
using System.Collections.Generic;

namespace WMBusinessTools.Extensions.StronglyTypedWrappers
{
    internal sealed class CreateOutgoingInvoiceFormValuesWrapper : StronglyTypedValuesWrapper
    {
        public const string Control2ReceiversWmidCommandFindPassport = "FindPassport";

        public int Control1OrderId
        {
            get => (int)(decimal) GetValue(0);
            set => SetValue("OrderId", (decimal) value);
        }

        public long Control2ReceiversWmid => long.Parse((string) GetValue(1));
        public string Control3PayTo => (string) GetValue(2);

        public decimal Control4Amount
        {
            get => (decimal) GetValue(3);
            set => SetValue("Amount", value);
        }

        public string Control5Description => (string) GetValue(4);
        public bool Control6SpecifyAdditionalParameters => (bool) GetValue(5);
        public string Control7Address => (string) GetValue(6);
        public bool Control8SpecifyPaymentPeriod => (bool) GetValue(7);

        public byte Control9PaymentPeriod
        {
            get => (byte)(decimal) GetValue(8);
            set => SetValue("PaymentPeriod", (decimal)value);
        }

        public bool Control10AllowPaymentWithProtection => (bool) GetValue(9);

        public byte Control11ProtectionPeriod
        {
            get => (byte)(decimal) GetValue(10);
            set => SetValue("ProtectionPeriod", (decimal)value);
        }

        public CreateOutgoingInvoiceFormValuesWrapper()
        {
        }

        public CreateOutgoingInvoiceFormValuesWrapper(List<object> values)
        {
            if (null == values)
                throw new ArgumentNullException(nameof(values));

            ApplyOutcomeValues(values);
        }
    }
}