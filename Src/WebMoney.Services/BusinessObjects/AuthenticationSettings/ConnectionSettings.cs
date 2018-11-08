using System;
using System.Xml.Serialization;
using AutoMapper;
using WebMoney.Services.Contracts.BusinessObjects;
using WebMoney.Services.Utils;

namespace WebMoney.Services.BusinessObjects
{
    [Serializable]
    public sealed class ConnectionSettings : IConnectionSettings, IEquatable<ConnectionSettings>, ICloneable
    {
        [XmlAttribute("connectionString")]
        public string ConnectionString { get; set; }

        [XmlAttribute("providerInvariantName")]
        public string ProviderInvariantName { get; set; }

        internal ConnectionSettings()
        {
        }

        public ConnectionSettings(string connectionString, string providerInvariantName)
        {
            ConnectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
            ProviderInvariantName = providerInvariantName ?? throw new ArgumentNullException(nameof(providerInvariantName));
        }

        public static ConnectionSettings Create(IConnectionSettings contractObject)
        {
            if (contractObject is ConnectionSettings businessObject)
                return businessObject;

            return Mapper.Map<ConnectionSettings>(contractObject);
        }

        public bool Equals(ConnectionSettings other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(ConnectionString, other.ConnectionString) && string.Equals(ProviderInvariantName, other.ProviderInvariantName);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is ConnectionSettings settings && Equals(settings);
        }

        public override int GetHashCode()
        {
            return 1;
        }

        public object Clone()
        {
            var o = MemberwiseClone();
            CloneUtility.CloneProperties(o);

            return o;
        }
    }
}
