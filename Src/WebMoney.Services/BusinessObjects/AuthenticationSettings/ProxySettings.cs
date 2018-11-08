using System;
using System.Xml.Serialization;
using AutoMapper;
using WebMoney.Services.Contracts.BusinessObjects;
using WebMoney.Services.Utils;

namespace WebMoney.Services.BusinessObjects
{
    [Serializable]
    public sealed class ProxySettings : IProxySettings, ICloneable, IEquatable<ProxySettings>
    {
        private string _host;
        private IProxyCredential _credential;

        [XmlAttribute("host")]
        public string Host
        {
            get => _host;
            set => _host = value ?? throw new ArgumentNullException(nameof(value));
        }

        [XmlAttribute("port")]
        public int Port { get; set; }

        [XmlElement("credentials")]
        public IProxyCredential Credential
        {
            get => _credential;
            set => _credential = ProxyCredential.Create(value);
        }

        internal ProxySettings()
        {
        }

        public ProxySettings(string host, int port)
        {
            Host = host ?? throw new ArgumentNullException(nameof(host));
            Port = port;
        }

        public static ProxySettings Create(IProxySettings contractObject)
        {
            if (null == contractObject)
                return null;

            if (contractObject is ProxySettings businessObject)
                return businessObject;

            return Mapper.Map<ProxySettings>(contractObject);
        }

        public bool Equals(ProxySettings other)
        {
            if (ReferenceEquals(null, other))
                return false;

            if (ReferenceEquals(this, other))
                return true;

            return string.Equals(_host, other.Host) && Port == other.Port && Equals(Credential, other.Credential);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;

            if (ReferenceEquals(this, obj))
                return true;

            return obj is ProxySettings proxySettings && Equals(proxySettings);
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