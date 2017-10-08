using System;
using System.Collections.Generic;

namespace WMBusinessTools.Extensions.StronglyTypedWrappers
{
    internal sealed class SendMessageFormValuesWrapper : StronglyTypedValuesWrapper
    {
        public const string Control1IdentifierCommandFindPassport = "FindPassport";

        public string Control1Identifier
        {
            get => (string) GetValue(0);
            set => SetValue("Identifier", value);
        }

        public string Control2Subject => (string) GetValue(1);
        public string Control3Message => (string) GetValue(2);

        public SendMessageFormValuesWrapper()
        {
        }

        public SendMessageFormValuesWrapper(List<object> values)
        {
            if (null == values)
                throw new ArgumentNullException(nameof(values));
            ApplyOutcomeValues(values);
        }
    }
}