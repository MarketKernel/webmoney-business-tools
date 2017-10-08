using System;
using System.Collections.Generic;

namespace WMBusinessTools.Extensions.StronglyTypedWrappers
{
    internal sealed class CreatePurseFormValuesWrapper : StronglyTypedValuesWrapper
    {
        public string Control1PurseType => (string) GetValue(0);
        public string Control2PurseName => (string) GetValue(1);

        public CreatePurseFormValuesWrapper()
        {
        }

        public CreatePurseFormValuesWrapper(List<object> values)
        {
            if (null == values)
                throw new ArgumentNullException(nameof(values));
            ApplyOutcomeValues(values);
        }
    }
}