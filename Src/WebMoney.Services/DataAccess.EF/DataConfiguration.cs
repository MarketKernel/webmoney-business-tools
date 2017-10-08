using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Data.Entity.SqlServerCompact;
using System.Data.SqlServerCe;

namespace WebMoney.Services.DataAccess.EF
{
    internal sealed class DataConfiguration : DbConfiguration
    {
        public const string SqlServerCompactProviderInvariantName = "System.Data.SqlServerCe.4.0";
        public const string SqlServerProviderInvariantName = "System.Data.SqlClient";

        public DataConfiguration()
        {
            SetProviderServices(SqlServerCompactProviderInvariantName, SqlCeProviderServices.Instance);
            SetProviderServices(SqlServerProviderInvariantName, SqlProviderServices.Instance);
            SetProviderFactory(SqlServerCompactProviderInvariantName, SqlCeProviderFactory.Instance);
        }
    }
}