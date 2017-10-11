using System.Data.Common;
using System.Data.Entity.Core.EntityClient;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Data.SqlServerCe;

namespace WebMoney.Services.DataAccess.EF
{
    internal class ProviderFactoryResolver : IDbProviderFactoryResolver
    {
        public DbProviderFactory ResolveProviderFactory(DbConnection connection)
        {
            if (connection is SqlCeConnection)
                return SqlCeProviderFactory.Instance;

            if (connection is SqlConnection)
                return SqlClientFactory.Instance;

            if (connection is EntityConnection)
                return EntityProviderFactory.Instance;

            return null;
        }
    }
}
