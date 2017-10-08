using System;
using System.Collections.Generic;

namespace WMBusinessTools.Extensions.StronglyTypedWrappers
{
    internal sealed class SetMerchantKeyFormValuesWrapper : StronglyTypedValuesWrapper
    {
        public string Control1MerchantKey => (string) GetValue(0);

        public SetMerchantKeyFormValuesWrapper()
        {
        }

        public SetMerchantKeyFormValuesWrapper(List<object> values)
        {
            if (null == values)
                throw new ArgumentNullException(nameof(values));
            ApplyOutcomeValues(values);
        }
    }
}