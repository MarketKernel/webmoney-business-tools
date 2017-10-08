using System;
using System.Collections.Generic;

namespace WMBusinessTools.Extensions.StronglyTypedWrappers
{
    internal sealed class TakeTrustFormValuesWrapper
    {
        public sealed class Step1 : StronglyTypedValuesWrapper
        {
            public const string Control7WMIDCommandFindPassport = "FindPassport";

            public const string Control5IdentifierTypeValuePhone = "Phone";
            public const string Control5IdentifierTypeValueWmid = "WmId";
            public const string Control5IdentifierTypeValueEmail = "Email";

            public const string Control9ConfirmationTypeValue1 = "1";
            public const string Control9ConfirmationTypeValue2 = "2";
            public string Control1StorePurse => (string) GetValue(0);

            public decimal Control2DailyAmountLimit
            {
                get => (decimal) GetValue(1);
                set => SetValue("DailyAmountLimit", value);
            }

            public decimal Control3WeeklyAmountLimit
            {
                get => (decimal) GetValue(2);
                set => SetValue("WeeklyAmountLimit", value);
            }

            public decimal Control4MonthlyAmountLimit
            {
                get => (decimal) GetValue(3);
                set => SetValue("MonthlyAmountLimit", value);
            }

            public string Control5IdentifierType => (string) GetValue(4);

            public string Control6Phone
            {
                get => (string) GetValue(5);
                set => SetValue("Phone", value);
            }

            public string Control7WMID
            {
                get => (string) GetValue(6);
                set => SetValue("WMID", value);
            }

            public string Control8Email
            {
                get => (string) GetValue(7);
                set => SetValue("Email", value);
            }

            public string Control9ConfirmationType => (string) GetValue(8);

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
            public const string Control3PurseCommandFindIdentifier = "FindIdentifier";
            public const string Control4SmsReferenceCommandGoTo = "GoTo";

            public string Control1RequestNumber
            {
                get => (string) GetValue(0);
                set => SetValue("RequestNumber", value);
            }

            public string Control2Message
            {
                get => (string) GetValue(1);
                set => SetValue("Message", value);
            }

            public string Control4SmsReference
            {
                get => (string) GetValue(2);
                set => SetValue("SmsReference", value);
            }

            public string Control5Code
            {
                get => (string) GetValue(3);
                set => SetValue("Code", value);
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