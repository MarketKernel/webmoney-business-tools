using System;
using System.Collections.Generic;

namespace WMBusinessTools.Extensions.StronglyTypedWrappers
{
    internal sealed class RedeemPaymerFormValuesWrapper : StronglyTypedValuesWrapper
    {
        public string Control1RedeemTo => (string) GetValue(0);
        public string Control2Number => (string) GetValue(1);
        public string Control3Code => (string) GetValue(2);

        public RedeemPaymerFormValuesWrapper()
        {
        }

        public RedeemPaymerFormValuesWrapper(List<object> values)
        {
            if (null == values)
                throw new ArgumentNullException(nameof(values));

            ApplyOutcomeValues(values);
        }
    }
}