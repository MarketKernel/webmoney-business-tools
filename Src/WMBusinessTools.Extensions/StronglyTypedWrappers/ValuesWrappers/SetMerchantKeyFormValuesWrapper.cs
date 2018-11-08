using System;
using System.Collections.Generic;

namespace WMBusinessTools.Extensions.StronglyTypedWrappers
{
    internal sealed class SetMerchantKeyFormValuesWrapper : StronglyTypedValuesWrapper
    {
        public bool Control1HasSecretKey
        {
            get => (bool) GetValue(0);
            set => SetValue("HasSecretKey", value);
        }

        public string Control2SecretKey
        {
            get => (string) GetValue(1);
            set => SetValue("SecretKey", value);
        }

        public bool Control3HasSecretKeyX20
        {
            get => (bool) GetValue(2);
            set => SetValue("HasSecretKeyX20", value);
        }

        public string Control4SecretKeyX20
        {
            get => (string) GetValue(3);
            set => SetValue("SecretKeyX20", value);
        }

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