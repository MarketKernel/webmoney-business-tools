using System;
using System.Data.Entity;
using System.Data.Entity.Migrations.History;
using System.Data.Entity.SqlServer;
using System.Data.Entity.SqlServerCompact;
using System.Data.SqlServerCe;
using MySql.Data.MySqlClient;
using Npgsql;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.EntityFramework;
using WebMoney.Services.Utils;

namespace WebMoney.Services.DataAccess.EF
{
    internal sealed class DataConfiguration : DbConfiguration
    {
        public const string SqlServerCompactProviderInvariantName = "System.Data.SqlServerCe.4.0";
        public const string SqlServerProviderInvariantName = "System.Data.SqlClient";
        public const string MySqlProviderInvariantName = "MySql.Data.MySqlClient";
        public const string PostgreSqlProviderInvariantName = "Npgsql";
        public const string OracleDBProviderInvariantName = "Oracle.ManagedDataAccess.Client";

        public DataConfiguration()
        {
            SetProviderFactory(SqlServerCompactProviderInvariantName, new SqlCeProviderFactory());
            SetProviderServices(SqlServerCompactProviderInvariantName, SqlCeProviderServices.Instance);

            SetProviderFactory(MySqlProviderInvariantName, new MySqlClientFactory());
            SetProviderServices(MySqlProviderInvariantName, new MySqlProviderServices());

            SetProviderFactory(PostgreSqlProviderInvariantName, NpgsqlFactory.Instance);
            SetProviderServices(PostgreSqlProviderInvariantName, NpgsqlServices.Instance);

            SetProviderFactory(OracleDBProviderInvariantName, OracleClientFactory.Instance);
            SetProviderServices(OracleDBProviderInvariantName, EFOracleProviderServices.Instance);
            SetHistoryContext(OracleDBProviderInvariantName, (connection, defaultSchema) =>
            {
                var userId = ConnectionStringParser.TryGetValue(connection.ConnectionString, "USER ID");

                if (null == userId)
                    throw new InvalidOperationException("null == userId");

                var schema = userId.ToUpper();

                return new HistoryContext(connection, schema);
            });

            SetProviderServices(SqlServerProviderInvariantName, SqlProviderServices.Instance);

            SetProviderFactoryResolver(new ProviderFactoryResolver());
        }
    }
}