using System;
using System.Collections.Generic;

namespace WMBusinessTools.Extensions.StronglyTypedWrappers
{
    internal sealed class CreateContractFormValuesWrapper : StronglyTypedValuesWrapper
    {
        public string Control1Name => (string) GetValue(0);
        public string Control2Text => (string) GetValue(1);
        public bool Control3LimitedAccess => (bool) GetValue(2);

        public string Control4AccessList
        {
            get => (string) GetValue(3);
            set => SetValue("AccessList", value);
        }

        public CreateContractFormValuesWrapper()
        {
        }

        public CreateContractFormValuesWrapper(List<object> values)
        {
            if (null == values)
                throw new ArgumentNullException(nameof(values));
            ApplyOutcomeValues(values);
        }
    }
}