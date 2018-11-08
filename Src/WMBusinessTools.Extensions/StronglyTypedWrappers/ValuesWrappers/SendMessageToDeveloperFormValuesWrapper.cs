using System;
using System.Collections.Generic;

namespace WMBusinessTools.Extensions.StronglyTypedWrappers
{
    internal sealed class SendMessageToDeveloperFormValuesWrapper : StronglyTypedValuesWrapper
    {
        public string Control1YourName => (string) GetValue(0);

        public string Control2InstallationReference
        {
            get => (string) GetValue(1);
            set => SetValue("InstallationReference", value);
        }

        public string Control3Message => (string) GetValue(2);

        public SendMessageToDeveloperFormValuesWrapper(List<object> values)
        {
            if (null == values)
                throw new ArgumentNullException(nameof(values));
            ApplyOutcomeValues(values);
        }
    }
}