using System.Data.Common;
using System.Data.Entity.Core.EntityClient;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Data.SqlServerCe;
using MySql.Data.MySqlClient;
using Npgsql;
using Oracle.ManagedDataAccess.Client;

namespace WebMoney.Services.DataAccess.EF
{
    internal class ProviderFactoryResolver : IDbProviderFactoryResolver
    {
        public DbProviderFactory ResolveProviderFactory(DbConnection connection)
        {
            if (connection is SqlConnection)
                return SqlClientFactory.Instance;

            if (connection is SqlCeConnection)
                return SqlCeProviderFactory.Instance;

            if (connection is MySqlConnection)
                return MySqlClientFactory.Instance;

            if (connection is NpgsqlConnection)
                return NpgsqlFactory.Instance;

            if (connection is OracleConnection)
                return OracleClientFactory.Instance;

            if (connection is EntityConnection)
                return EntityProviderFactory.Instance;

            return null;
        }
    }
}
