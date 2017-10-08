using System;
using System.Collections.Generic;

namespace WMBusinessTools.Extensions.StronglyTypedWrappers
{
    internal sealed class LightKeySettingsFormValuesWrapper : StronglyTypedValuesWrapper
    {
        public object Control1Certificate
        {
            get => GetValue(0);
            set => SetValue("Certificate", value);
        }

        public LightKeySettingsFormValuesWrapper()
        {
        }

        public LightKeySettingsFormValuesWrapper(List<object> values)
        {
            if (null == values)
                throw new ArgumentNullException(nameof(values));
            ApplyOutcomeValues(values);
        }
    }
}