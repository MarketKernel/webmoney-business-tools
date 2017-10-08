using System;
using System.Collections.Generic;

namespace WMBusinessTools.Extensions.StronglyTypedWrappers
{
    internal sealed class TakePaymentFormValuesWrapper
    {
        public sealed class Step1 : StronglyTypedValuesWrapper
        {
            public const string Control5WMIDCommandFindPassport = "FindPassport";

            public const string Control3IdentifierTypeValuePhone = "Phone";
            public const string Control3IdentifierTypeValueWmid = "WmId";
            public const string Control3IdentifierTypeValueEmail = "Email";

            public const string Control8ConfirmationTypeValue3 = "3";
            public const string Control8ConfirmationTypeValue1 = "1";
            public const string Control8ConfirmationTypeValue2 = "2";
            public const string Control8ConfirmationTypeValue4 = "4";
            public const string Control8ConfirmationTypeValue5 = "5";
            public string Control1StorePurse => (string) GetValue(0);

            public int Control2OrderId
            {
                get => (int)(decimal) GetValue(1);
                set => SetValue("OrderId", (decimal)value);
            }

            public string Control3IdentifierType => (string) GetValue(2);

            public string Control4Phone
            {
                get => (string) GetValue(3);
                set => SetValue("Phone", value);
            }

            public string Control5WMID
            {
                get => (string) GetValue(4);
                set => SetValue("WMID", value);
            }

            public string Control6Email
            {
                get => (string) GetValue(5);
                set => SetValue("Email", value);
            }

            public decimal Control7PaymentAmount
            {
                get => (decimal) GetValue(6);
                set => SetValue("PaymentAmount", value);
            }

            public string Control8ConfirmationType => (string) GetValue(7);

            public string Control9Description => (string) GetValue(8);

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
            public string Control1InvoiceId
            {
                get => (string) GetValue(0);
                set => SetValue("InvoiceId", value);
            }

            public string Control2Message
            {
                get => (string) GetValue(1);
                set => SetValue("Message", value);
            }

            public string Control3Code
            {
                get => (string) GetValue(2);
                set => SetValue("Code", value);
            }

            public bool Control4CancelInvoice => (bool) GetValue(3);

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