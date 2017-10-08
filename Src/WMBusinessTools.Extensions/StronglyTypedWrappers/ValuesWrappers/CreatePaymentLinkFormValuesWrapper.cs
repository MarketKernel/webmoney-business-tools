using System;
using System.Collections.Generic;

namespace WMBusinessTools.Extensions.StronglyTypedWrappers
{
    internal sealed class CreatePaymentLinkFormValuesWrapper
    {
        public sealed class Step1 : StronglyTypedValuesWrapper
        {
            public string Control1StorePurse => (string) GetValue(0);

            public int Control2OrderId
            {
                get => (int)(decimal) GetValue(1);
                set => SetValue("OrderId", (decimal)value);
            }

            public decimal Control3PaymentAmount
            {
                get => (decimal) GetValue(2);
                set => SetValue("PaymentAmount", value);
            }

            public int Control4ValidityPeriod => (int) (decimal) GetValue(3);
            public string Control5Description => (string) GetValue(4);

            public Step1()
            {
            }

            public Step1(List<object> values)
            {
                if (null == values)
                    throw new ArgumentNullException(nameof(values));
                ApplyOutcomeValues(values);
            }
        }

        public sealed class Step2 : StronglyTypedValuesWrapper
        {
            public const string Control1PaymentLinkCommandCopy = "Copy";

            public string Control1PaymentLink
            {
                get => (string) GetValue(0);
                set => SetValue("PaymentLink", value);
            }

            public Step2()
            {
            }

            public Step2(List<object> values)
            {
                if (null == values)
                    throw new ArgumentNullException(nameof(values));
                ApplyOutcomeValues(values);
            }
        }
    }
}