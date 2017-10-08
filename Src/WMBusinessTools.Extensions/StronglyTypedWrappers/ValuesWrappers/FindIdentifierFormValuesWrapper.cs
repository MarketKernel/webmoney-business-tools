using System;
using System.Collections.Generic;

namespace WMBusinessTools.Extensions.StronglyTypedWrappers
{
    internal sealed class FindIdentifierFormValuesWrapper
    {
        public sealed class Step1 : StronglyTypedValuesWrapper
        {
            public string Control1Purse
            {
                get => (string) GetValue(0);
                set => SetValue("Purse", value);
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
            public const string Control1WmIdCommandFindPassport = "FindPassport";

            public string Control1WmId
            {
                get => (string) GetValue(0);
                set => SetValue("WmId", value);
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