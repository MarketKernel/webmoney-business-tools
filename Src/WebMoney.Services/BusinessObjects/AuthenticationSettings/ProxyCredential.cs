using System;
using System.Xml.Serialization;
using AutoMapper;
using WebMoney.Services.Contracts.BusinessObjects;
using WebMoney.Services.Utils;

namespace WebMoney.Services.BusinessObjects
{
    [Serializable]
    public sealed class ProxyCredential : IProxyCredential, ICloneable, IEquatable<ProxyCredential>
    {
        private string _username;
        private string _password;

        [XmlAttribute("username")]
        public string Username
        {
            get => _username;
            set => _username = value ?? throw new ArgumentNullException(nameof(value));
        }

        [XmlAttribute("password")]
        public string Password
        {
            get => _password;
            set => _password = value ?? throw new ArgumentNullException(nameof(value));
        }

        internal ProxyCredential()
        {
        }

        public ProxyCredential(string username, string password)
        {
            Username = username ?? throw new ArgumentNullException(nameof(username));
            Password = password ?? throw new ArgumentNullException(nameof(password));
        }

        public static ProxyCredential Create(IProxyCredential contractObject)
        {
            if (null == contractObject)
                return null;

            if (contractObject is ProxyCredential businessObject)
                return businessObject;

            return Mapper.Map<ProxyCredential>(contractObject);
        }

        public bool Equals(ProxyCredential other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return string.Equals(_username, other.Username) && string.Equals(_password, other.Password);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is ProxyCredential credential && Equals(credential);
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
