using System.Data.Entity.Migrations;
using System.IO;
using WebMoney.Services.DataAccess.EF;

namespace WebMoney.Services.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<DataContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;

            //SetSqlGenerator("Npgsql", new NpgsqlMigrationSqlGenerator());
            //SetSqlGenerator("MySql.Data.MySqlClient", new MySqlMigrationSqlGenerator());

            MigrationsDirectory = Path.Combine(DefaultMigrationsDirectory, nameof(SqlServer));
            MigrationsNamespace = typeof(SqlServer.V1).Namespace;

            //MigrationsDirectory = Path.Combine(DefaultMigrationsDirectory, nameof(SqlCE));
            //MigrationsNamespace = typeof(SqlCE.V1).Namespace;
        }

        protected override void Seed(DataContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
        }
    }
}
