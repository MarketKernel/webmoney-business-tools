using System;
using System.Collections.Generic;

namespace WMBusinessTools.Extensions.StronglyTypedWrappers
{
    internal sealed class FinishProtectionFormValuesWrapper : StronglyTypedValuesWrapper
    {
        public const string Control1SourcePurseCommandFindIdentifier = "FindIdentifier";

        public string Control1SourcePurse
        {
            get => (string) GetValue(0);
            set => SetValue("SourcePurse", value);
        }

        public string Control2TargetPurse
        {
            get => (string) GetValue(1);
            set => SetValue("TargetPurse", value);
        }

        public decimal? Control3Amount
        {
            get => (decimal?) GetValue(2);
            set => SetValue("Amount", value);
        }

        public string Control4Description
        {
            get => (string) GetValue(3);
            set => SetValue("Description", value);
        }

        public string Control5Code
        {
            get => (string) GetValue(4);
            set => SetValue("Code", value);
        }

        public bool Control6HoldingFeatureIsUsed => (bool) GetValue(5);

        public FinishProtectionFormValuesWrapper()
        {
        }

        public FinishProtectionFormValuesWrapper(List<object> values)
        {
            if (null == values)
                throw new ArgumentNullException(nameof(values));
            ApplyOutcomeValues(values);
        }
    }
}