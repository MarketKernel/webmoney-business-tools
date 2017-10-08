using System;
using System.Collections.Generic;

namespace WMBusinessTools.Extensions.StronglyTypedWrappers
{
    internal sealed class ClassicKeySettingsFormValuesWrapper : StronglyTypedValuesWrapper
    {
        public const string Control1PathToKeysBackupFileCommandSelectFile = "SelectFile";

        public string Control1PathToKeysBackupFile => (string) GetValue(0);
        public string Control2PasswordToBackupFile => (string) GetValue(1);

        public ClassicKeySettingsFormValuesWrapper()
        {
        }

        public ClassicKeySettingsFormValuesWrapper(List<object> values)
        {
            if (null == values)
                throw new ArgumentNullException(nameof(values));

            ApplyOutcomeValues(values);
        }
    }
}