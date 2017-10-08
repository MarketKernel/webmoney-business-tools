using System;
using WebMoney.Services.Contracts.BusinessObjects;

namespace WMBusinessTools.Extensions.BusinessObjects
{
    internal sealed class ConnectionSettings : IConnectionSettings
    {
        public string ProviderInvariantName { get; }
        public string ConnectionString { get; }

        public ConnectionSettings(string providerInvariantName, string connectionString)
        {
            ProviderInvariantName = providerInvariantName ?? throw new ArgumentNullException(nameof(providerInvariantName));
            ConnectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public bool Equals(IConnectionSettings other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(ProviderInvariantName, other.ProviderInvariantName) &&
                   string.Equals(ConnectionString, other.ConnectionString);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is IConnectionSettings && Equals((IConnectionSettings)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((ProviderInvariantName != null ? ProviderInvariantName.GetHashCode() : 0) * 397) ^ (ConnectionString != null ? ConnectionString.GetHashCode() : 0);
            }
        }


        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
