using System;
using System.Collections.Generic;

namespace WMBusinessTools.Extensions.StronglyTypedWrappers
{
    internal sealed class FindMerchantTransferFormValuesWrapper
    {
        public sealed class Step1 : StronglyTypedValuesWrapper
        {
            public const string Control3NumberTypeValueAuto = "Auto";
            public const string Control3NumberTypeValueOrderid = "OrderId";
            public const string Control3NumberTypeValueInvoiceprimaryid = "InvoicePrimaryId";
            public const string Control3NumberTypeValueTransferprimaryid = "TransferPrimaryId";

            public string Control1FromPurse => (string) GetValue(0);

            public long Control2Number
            {
                get => (long)(decimal) GetValue(1);
                set => SetValue("Number", (decimal)value);
            }

            public string Control3NumberType
            {
                get => (string) GetValue(2);
                set => SetValue("NumberType", value);
            }

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
            public object Control1Payment
            {
                get => GetValue(0);
                set => SetValue("Payment", value);
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