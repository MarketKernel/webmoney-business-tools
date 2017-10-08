using System;
using System.Collections.Generic;

namespace WMBusinessTools.Extensions.StronglyTypedWrappers
{
    internal sealed class DbSettingsFormValuesWrapper : StronglyTypedValuesWrapper
    {
        public const string Control2PathToDatabaseCommandSelectFile = "SelectFile";

        public const string Control1ProviderValueSystemDataSqlserverce40 = "System.Data.SqlServerCe.4.0";
        public const string Control1ProviderValueSystemDataSqlclient = "System.Data.SqlClient";

        public string Control1Provider
        {
            get => (string) GetValue(0);
            set => SetValue("Provider", value);
        }

        public string Control2PathToDatabase
        {
            get => (string) GetValue(1);
            set => SetValue("PathToDatabase", value);
        }

        public bool Control3UsePassword
        {
            get => (bool) GetValue(2);
            set => SetValue("UsePassword", value);
        }

        public string Control4Password
        {
            get => (string) GetValue(3);
            set => SetValue("Password", value);
        }

        public string Control5PasswordConfirmation
        {
            get => (string) GetValue(4);
            set => SetValue("PasswordConfirmation", value);
        }

        public string Control6ConnectionString
        {
            get => (string) GetValue(5);
            set => SetValue("ConnectionString", value);
        }

        public DbSettingsFormValuesWrapper()
        {
        }

        public DbSettingsFormValuesWrapper(List<object> values)
        {
            if (null == values)
                throw new ArgumentNullException(nameof(values));
            ApplyOutcomeValues(values);
        }
    }
}