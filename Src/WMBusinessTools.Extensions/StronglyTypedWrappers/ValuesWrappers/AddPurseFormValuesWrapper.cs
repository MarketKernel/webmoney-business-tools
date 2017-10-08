using System;
using System.Collections.Generic;

namespace WMBusinessTools.Extensions.StronglyTypedWrappers
{
    internal sealed class AddPurseFormValuesWrapper : StronglyTypedValuesWrapper
    {
        public const string Control1PurseNumberCommandFindIdentifier = "FindIdentifier";
        public string Control1PurseNumber => (string) GetValue(0);
        public string Control2Alias => (string) GetValue(1);

        public AddPurseFormValuesWrapper()
        {
        }

        public AddPurseFormValuesWrapper(List<object> values)
        {
            if (null == values)
                throw new ArgumentNullException(nameof(values));
            ApplyOutcomeValues(values);
        }
    }
}