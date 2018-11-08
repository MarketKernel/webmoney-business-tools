using System;
using System.Data.Entity.Migrations;
using System.IO;
using MySql.Data.Entity;
using Npgsql;
using WebMoney.Services.DataAccess.EF;

namespace WebMoney.Services.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<DataContext>
    {
        // SqlCE
        private static readonly string SqlCEMigrationsDirectory = Path.Combine(DefaultMigrationsDirectory, "SqlCE");
        private static readonly string SqlCEMigrationsNamespace = "WebMoney.Services.Migrations.SqlCE";

        // SqlServer
        private static readonly string SqlServerMigrationsDirectory = Path.Combine(DefaultMigrationsDirectory, "SqlServer");
        private static readonly string SqlServerMigrationsNamespace = "WebMoney.Services.Migrations.SqlServer";

        // MySql
        private static readonly string MySqlMigrationsDirectory = Path.Combine(DefaultMigrationsDirectory, "MySql");
        private static readonly string MySqlMigrationsNamespace = "WebMoney.Services.Migrations.MySql";

        // PostgreSql
        private static readonly string PostgreSqlMigrationsDirectory = Path.Combine(DefaultMigrationsDirectory, "PostgreSql");
        private static readonly string PostgreSqlMigrationsNamespace = "WebMoney.Services.Migrations.PostgreSql";

        // OracleDB
        private static readonly string OracleDBMigrationsDirectory = Path.Combine(DefaultMigrationsDirectory, "OracleDB");
        private static readonly string OracleDBMigrationsNamespace = "WebMoney.Services.Migrations.OracleDB";

        public Configuration()
        {
            AutomaticMigrationsEnabled = false;

            SetSqlGenerator("Npgsql", new NpgsqlMigrationSqlGenerator());
            SetSqlGenerator("MySql.Data.MySqlClient", new MySqlMigrationSqlGenerator());

            SetDirectoryAndNamespace(MigrationConfiguration.ProviderInvariantName);
        }

        internal void SetDirectoryAndNamespace(string providerInvariantName)
        {
            if (null == providerInvariantName)
                throw new ArgumentNullException(nameof(providerInvariantName));

            switch (providerInvariantName)
            {
                case DataConfiguration.SqlServerCompactProviderInvariantName:
                    MigrationsDirectory = SqlCEMigrationsDirectory;
                    MigrationsNamespace = SqlCEMigrationsNamespace;
                    break;
                case DataConfiguration.SqlServerProviderInvariantName:
                    MigrationsDirectory = SqlServerMigrationsDirectory;
                    MigrationsNamespace = SqlServerMigrationsNamespace;
                    break;
                case DataConfiguration.MySqlProviderInvariantName:
                    MigrationsDirectory = MySqlMigrationsDirectory;
                    MigrationsNamespace = MySqlMigrationsNamespace;
                    break;
                case DataConfiguration.PostgreSqlProviderInvariantName:
                    MigrationsDirectory = PostgreSqlMigrationsDirectory;
                    MigrationsNamespace = PostgreSqlMigrationsNamespace;
                    break;
                case DataConfiguration.OracleDBProviderInvariantName:
                    MigrationsDirectory = OracleDBMigrationsDirectory;
                    MigrationsNamespace = OracleDBMigrationsNamespace;
                    break;
                default:
                    throw new InvalidOperationException(
                        $"MigrationConfiguration.ProviderInvariantName={MigrationConfiguration.ProviderInvariantName}");
            }
        }

        protected override void Seed(DataContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
        }
    }
}
