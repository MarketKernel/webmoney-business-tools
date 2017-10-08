using System;
using System.Collections.Generic;

namespace WMBusinessTools.Extensions.StronglyTypedWrappers
{
    internal sealed class SendSmsFormValuesWrapper : StronglyTypedValuesWrapper
    {
        public string Control1PhoneNumber => (string) GetValue(0);
        public string Control2Message => (string) GetValue(1);
        public bool Control3UseTransliteration => (bool) GetValue(2);

        public SendSmsFormValuesWrapper()
        {
        }

        public SendSmsFormValuesWrapper(List<object> values)
        {
            if (null == values)
                throw new ArgumentNullException(nameof(values));
            ApplyOutcomeValues(values);
        }
    }
}