using System;
using System.Collections.Generic;

namespace WMBusinessTools.Extensions.StronglyTypedWrappers
{
    internal sealed class ContractDetailsFormValuesWrapper : StronglyTypedValuesWrapper
    {
        public string Control1Name
        {
            get => (string) GetValue(0);
            set => SetValue("Name", value);
        }

        public string Control2Text
        {
            get => (string) GetValue(1);
            set => SetValue("Text", value);
        }

        public bool Control3HasLimitedAccess
        {
            get => (bool) GetValue(2);
            set => SetValue("HasLimitedAccess", value);
        }

        public object Control4Details
        {
            get => GetValue(3);
            set => SetValue("Details", value);
        }

        public ContractDetailsFormValuesWrapper()
        {
        }

        public ContractDetailsFormValuesWrapper(List<object> values)
        {
            if (null == values)
                throw new ArgumentNullException(nameof(values));
            ApplyOutcomeValues(values);
        }
    }
}