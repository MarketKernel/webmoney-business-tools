using System;
using System.Data;
using System.Text;
using Unity;
using WebMoney.Services.Contracts;
using WebMoney.Services.Contracts.BusinessObjects;
using WMBusinessTools.Extensions.BusinessObjects;
using WMBusinessTools.Extensions.Properties;
using WMBusinessTools.Extensions.StronglyTypedWrappers;

namespace WMBusinessTools.Extensions.Services
{
    internal sealed class DbSettingsService : Service
    {
        class SqlCompactConnectionParts
        {
            public string DataSource { get; private set; }
            public string Password { get; private set; }

            private SqlCompactConnectionParts()
            {
            }

            public static SqlCompactConnectionParts Parse(string connectionStringWithProvider)
            {
                if (null == connectionStringWithProvider)
                    throw new ArgumentNullException(nameof(connectionStringWithProvider));

                var parts = connectionStringWithProvider.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries);

                string dataSource = null;
                string password = null;

                foreach (var part in parts)
                {
                    var nameValue = part.Trim().Split(new[] { "=" }, StringSplitOptions.RemoveEmptyEntries);

                    if (2 != nameValue.Length)
                        throw new InvalidOperationException("2 != nameValue.Length");

                    var name = nameValue[0].Trim();
                    var value = nameValue[1].Trim();

                    switch (name)
                    {
                        case "Data Source":
                            dataSource = value;
                            break;
                        case "Password":
                            password = value;
                            break;
                    }
                }

                return new SqlCompactConnectionParts
                {
                    DataSource = dataSource,
                    Password = password
                };
            }
        }

        public DbSettingsService(IUnityContainer container)
            : base(container)
        {
        }

        public void CheckConnectionSettings(IConnectionSettings connectionString)
        {
            if (null == connectionString)
                throw new ArgumentNullException(nameof(connectionString));

            var entranceService = Container.Resolve<IEntranceService>();

            try
            {
                entranceService.Connect(connectionString.ConnectionString, connectionString.ProviderInvariantName);
            }
            catch (Exception e) when (e is DataException || e is InvalidOperationException)
            {
                throw new InvalidOperationException(Resources.DbSettingsService_CheckConnectionSettings_Unable_connect_to_database_, e);
            }
        }

        internal static IConnectionSettings ExtractConnectionSettings(DbSettingsFormValuesWrapper valuesWrapper)
        {
            if (null == valuesWrapper)
                throw new ArgumentNullException(nameof(valuesWrapper));

            switch (valuesWrapper.Control1Provider)
            {
                case DbSettingsFormValuesWrapper.Control1ProviderValueSystemDataSqlserverce40:
                {
                    var connectionBuilder = new StringBuilder();

                    connectionBuilder.Append($"Data Source={valuesWrapper.Control2PathToDatabase};");

                    if (valuesWrapper.Control3UsePassword)
                        connectionBuilder.Append($" Password={valuesWrapper.Control4Password};");

                    return new ConnectionSettings(valuesWrapper.Control1Provider, connectionBuilder.ToString());
                }
                case DbSettingsFormValuesWrapper.Control1ProviderValueSystemDataSqlclient:
                case DbSettingsFormValuesWrapper.Control1ProviderValueMysqlDataMysqlclient:
                case DbSettingsFormValuesWrapper.Control1ProviderValueNpgsql:
                case DbSettingsFormValuesWrapper.Control1ProviderValueOracleManageddataaccessClient:
                    return new ConnectionSettings(valuesWrapper.Control1Provider,
                        valuesWrapper.Control6ConnectionString);
                default:
                    throw new InvalidOperationException("valuesWrapper.Control1Provider == " +
                                                        valuesWrapper.Control1Provider);
            }
        }

        internal static DbSettingsFormValuesWrapper MapToValuesWrapper(IConnectionSettings connectionSettings)
        {
            if (null == connectionSettings)
                throw new ArgumentNullException(nameof(connectionSettings));

            switch (connectionSettings.ProviderInvariantName)
            {
                case DbSettingsFormValuesWrapper.Control1ProviderValueSystemDataSqlserverce40:
                {
                    var connectionParts = SqlCompactConnectionParts.Parse(connectionSettings.ConnectionString);

                    return new DbSettingsFormValuesWrapper
                    {
                        Control1Provider = connectionSettings.ProviderInvariantName,
                        Control2PathToDatabase = connectionParts.DataSource,
                        Control3UsePassword = null != connectionParts.Password,
                        Control4Password = connectionParts.Password ?? string.Empty,
                        Control5PasswordConfirmation = connectionParts.Password ?? string.Empty
                    };
                }
                case DbSettingsFormValuesWrapper.Control1ProviderValueSystemDataSqlclient:
                case DbSettingsFormValuesWrapper.Control1ProviderValueMysqlDataMysqlclient:
                case DbSettingsFormValuesWrapper.Control1ProviderValueNpgsql:
                case DbSettingsFormValuesWrapper.Control1ProviderValueOracleManageddataaccessClient:
                    return new DbSettingsFormValuesWrapper
                    {
                        Control1Provider = connectionSettings.ProviderInvariantName,
                        Control6ConnectionString = connectionSettings.ConnectionString
                    };
                default:
                    throw new InvalidOperationException("connectionSettings.ProviderInvariantName == " +
                                                        connectionSettings.ProviderInvariantName);
            }
        }

        public static string ToConnectionStringWithProvider(IConnectionSettings connectionSettings)
        {
            if (null == connectionSettings)
                throw new ArgumentNullException(nameof(connectionSettings));

            return $"Provider={connectionSettings.ProviderInvariantName}; {connectionSettings.ConnectionString}";
        }

        public static IConnectionSettings ParseConnectionSettings(string connectionStringWithProvider)
        {
            if (null == connectionStringWithProvider)
                throw new ArgumentNullException(nameof(connectionStringWithProvider));

            var parts = connectionStringWithProvider.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries);

            string provider = null;
            var stringBuilder = new StringBuilder();

            bool first = true;

            foreach (var part in parts)
            {
                var nameValue = part.Trim().Split(new[] { "=" }, StringSplitOptions.RemoveEmptyEntries);

                if (2 != nameValue.Length)
                    throw new InvalidOperationException("Wrong connection string (2 != nameValue.Length)!");

                var name = nameValue[0].Trim();
                var value = nameValue[1].Trim();

                switch (name)
                {
                    case "Provider":
                        provider = value;
                        break;
                    default:
                    {
                        if (first)
                            first = false;
                        else
                            stringBuilder.Append(";");

                        stringBuilder.AppendFormat("{0}={1}", name, value);
                    }
                        break;
                }
            }

            if (null == provider)
                throw new InvalidOperationException("Wrong connection string (null == provider)!");

            return new ConnectionSettings(provider, stringBuilder.ToString());
        }
    }
}
