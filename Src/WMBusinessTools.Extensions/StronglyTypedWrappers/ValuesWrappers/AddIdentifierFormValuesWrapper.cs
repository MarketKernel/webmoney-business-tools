using System;
using System.Collections.Generic;

namespace WMBusinessTools.Extensions.StronglyTypedWrappers
{
    internal sealed class AddIdentifierFormValuesWrapper : StronglyTypedValuesWrapper
    {
        public const string Control1WmidCommandFindPassport = "FindPassport";

        public string Control1Wmid => (string) GetValue(0);
        public string Control2Alias => (string) GetValue(1);

        public AddIdentifierFormValuesWrapper()
        {
        }

        public AddIdentifierFormValuesWrapper(List<object> values)
        {
            if (null == values)
                throw new ArgumentNullException(nameof(values));

            ApplyOutcomeValues(values);
        }
    }
}