using System;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Validation;
using System.Text;
using MySql.Data.Entity;
using Npgsql;
using Oracle.ManagedDataAccess.EntityFramework;
using WebMoney.Services.BusinessObjects;
using WebMoney.Services.Contracts.BusinessObjects;
using WebMoney.Services.Utils;

namespace WebMoney.Services.DataAccess.EF
{
    internal sealed class DataContext : DbContext
    {
        private readonly string _schema;

        internal static IConnectionSettings ConnectionSettings { get; set; }

        public DbSet<IdentifierSummary> IdentifierSummaries { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Trust> Trusts { get; set; }
        public DbSet<Transfer> Transfers { get; set; }
        public DbSet<Contract> Contracts { get; set; }
        public DbSet<ContractSignature> ContractSignatures { get; set; }
        public DbSet<TransferBundle> TransferBundles { get; set; }
        public DbSet<PreparedTransfer> PreparedTransfers { get; set; }

        // Вспомогательные
        public DbSet<Certificate> Certificates { get; set; }
        public DbSet<AttachedIdentifierSummary> AttachedIdentifierSummaries { get; set; }
        public DbSet<PurseSummary> PurseSummaries { get; set; }

        // Системные
        public DbSet<Record> Records { get; set; }

        static DataContext()
        {
            switch (MigrationConfiguration.ProviderInvariantName)
            {
                case DataConfiguration.SqlServerCompactProviderInvariantName:
                    ConnectionSettings = new ConnectionSettings("Data Source=D:\\_TEMP\\017674283968-v2.sdf; Persist Security Info=False;", DataConfiguration.SqlServerCompactProviderInvariantName);
                    break;
                case DataConfiguration.SqlServerProviderInvariantName:
                    ConnectionSettings = new ConnectionSettings("Data Source=DESKTOP-53N4OHF\\SQLEXPRESS;Initial Catalog=wmbt6;Integrated Security=True", DataConfiguration.SqlServerProviderInvariantName);
                    break;
                case DataConfiguration.MySqlProviderInvariantName:
                    ConnectionSettings = new ConnectionSettings("Server=127.0.0.1;Port=13306;Database=wmbt2;Uid=wmbtuser2;Pwd=msqEer2Pwd77;", DataConfiguration.MySqlProviderInvariantName);
                    break;
                case DataConfiguration.PostgreSqlProviderInvariantName:
                    ConnectionSettings = new ConnectionSettings("Server=127.0.0.1; Port=15432; Database=wmbt;User Id=wmbtuser; Password=psqEer2Pwd77;", DataConfiguration.PostgreSqlProviderInvariantName);
                    break;
                case DataConfiguration.OracleDBProviderInvariantName:
                    ConnectionSettings = new ConnectionSettings("DATA SOURCE=144.21.94.140:1521/PDB1.608775938.oraclecloud.internal;PERSIST SECURITY INFO=True;USER ID=wmbtuser;Password=OsqEer2Pwd77#", DataConfiguration.OracleDBProviderInvariantName);
                    break;
                default:
                    throw new InvalidOperationException(
                        $"MigrationConfiguration.ProviderInvariantName={MigrationConfiguration.ProviderInvariantName}");
            }
        }

        public DataContext()
            : this(null)
        {
            
        }

        public DataContext(IConnectionSettings connectionSettings)
            : base(BuildConnection(connectionSettings), true)
        {
            if (null == connectionSettings)
                connectionSettings = ConnectionSettings;

            switch (connectionSettings.ProviderInvariantName)
            {
                case DataConfiguration.OracleDBProviderInvariantName:

                    var userId = ConnectionStringParser.TryGetValue(connectionSettings.ConnectionString, "USER ID");

                    if (null == userId)
                        throw new InvalidOperationException("null == userId");

                    _schema = userId.ToUpper();

                    break;
            }
        }

        private static DbConnection BuildConnection(IConnectionSettings connectionSettings)
        {
            if (null == connectionSettings)
                connectionSettings = ConnectionSettings;

            switch (connectionSettings.ProviderInvariantName)
            {
                case DataConfiguration.SqlServerCompactProviderInvariantName:
                {
                    var connectionFactory =
                        new SqlCeConnectionFactory(DataConfiguration.SqlServerCompactProviderInvariantName);
                    return connectionFactory.CreateConnection(connectionSettings.ConnectionString);
                }
                case DataConfiguration.SqlServerProviderInvariantName:
                {
                    var connectionFactory = new SqlConnectionFactory();
                    return connectionFactory.CreateConnection(connectionSettings.ConnectionString);
                }
                case DataConfiguration.MySqlProviderInvariantName:
                {
                    var connectionFactory = new MySqlConnectionFactory();
                    return connectionFactory.CreateConnection(connectionSettings.ConnectionString);
                }
                case DataConfiguration.PostgreSqlProviderInvariantName:
                {
                    var connectionFactory = new NpgsqlConnectionFactory();
                    return connectionFactory.CreateConnection(connectionSettings.ConnectionString);
                }
                case DataConfiguration.OracleDBProviderInvariantName:
                {
                    var connectionFactory = new OracleConnectionFactory();
                    return connectionFactory.CreateConnection(connectionSettings.ConnectionString);
                }
                default:
                    throw new InvalidOperationException("connectionSettings.ProviderInvariantName == " +
                                                        connectionSettings.ProviderInvariantName);
            }
        }

        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (DbEntityValidationException exception)
            {
                var message = new StringBuilder();

                foreach (DbEntityValidationResult validationResult in exception.EntityValidationErrors)
                {
                    string entityName = validationResult.Entry.Entity.GetType().Name;

                    foreach (DbValidationError error in validationResult.ValidationErrors)
                    {
                        message.AppendLine(entityName + "." + error.PropertyName + ": " + error.ErrorMessage);
                    }
                }

                throw new DbEntityValidationException(message.ToString(), exception);
            }
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            if (null == modelBuilder)
                throw new ArgumentNullException(nameof(modelBuilder));

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            //modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            if (null != _schema)
                modelBuilder.HasDefaultSchema(_schema);

            // AccountEntity
            modelBuilder.Entity<Account>().Property(a => a.Amount).HasPrecision(16, 2);

            modelBuilder.Entity<Account>().Property(a => a.DayLimit).HasPrecision(16, 2);
            modelBuilder.Entity<Account>().Property(a => a.WeekLimit).HasPrecision(16, 2);
            modelBuilder.Entity<Account>().Property(a => a.MonthLimit).HasPrecision(16, 2);
            modelBuilder.Entity<Account>().Property(a => a.DayTotalAmount).HasPrecision(16, 2);
            modelBuilder.Entity<Account>().Property(a => a.WeekTotalAmount).HasPrecision(16, 2);
            modelBuilder.Entity<Account>().Property(a => a.MonthTotalAmount).HasPrecision(16, 2);

            // TrustEntity
            modelBuilder.Entity<Trust>().Property(p => p.DayLimit).HasPrecision(16, 2);
            modelBuilder.Entity<Trust>().Property(p => p.WeekLimit).HasPrecision(16, 2);
            modelBuilder.Entity<Trust>().Property(p => p.MonthLimit).HasPrecision(16, 2);
            modelBuilder.Entity<Trust>().Property(p => p.DayTotalAmount).HasPrecision(16, 2);
            modelBuilder.Entity<Trust>().Property(p => p.WeekTotalAmount).HasPrecision(16, 2);
            modelBuilder.Entity<Trust>().Property(p => p.MonthTotalAmount).HasPrecision(16, 2);

            // TransferEntity
            modelBuilder.Entity<Transfer>().Property(p => p.Amount).HasPrecision(16, 2);
            modelBuilder.Entity<Transfer>().Property(p => p.Commission).HasPrecision(16, 2);
            modelBuilder.Entity<Transfer>().Property(p => p.Balance).HasPrecision(16, 2);

            // TransferBundleEntity
            modelBuilder.Entity<TransferBundle>().Property(p => p.RegisteredTotalAmount).HasPrecision(16, 2);
            modelBuilder.Entity<TransferBundle>().Property(p => p.ProcessedTotalAmount).HasPrecision(16, 2);
            modelBuilder.Entity<TransferBundle>().Property(p => p.InterruptedTotalAmount).HasPrecision(16, 2);
            modelBuilder.Entity<TransferBundle>().Property(p => p.CompletedTotalAmount).HasPrecision(16, 2);

            // PreparedTransferEntity
            modelBuilder.Entity<PreparedTransfer>().Property(p => p.Amount).HasPrecision(16, 2);
        }
    }
}
