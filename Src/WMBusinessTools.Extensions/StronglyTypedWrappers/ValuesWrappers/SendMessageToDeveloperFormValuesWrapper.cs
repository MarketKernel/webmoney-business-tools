using System;
using System.Collections.Generic;

namespace WMBusinessTools.Extensions.StronglyTypedWrappers
{
    internal sealed class SendMessageToDeveloperFormValuesWrapper : StronglyTypedValuesWrapper
    {
        public string Control1YourName => (string) GetValue(0);
        public string Control2Message => (string) GetValue(1);

        public SendMessageToDeveloperFormValuesWrapper()
        {
        }

        public SendMessageToDeveloperFormValuesWrapper(List<object> values)
        {
            if (null == values)
                throw new ArgumentNullException(nameof(values));
            ApplyOutcomeValues(values);
        }
    }
}