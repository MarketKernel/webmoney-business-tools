using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using AutoMapper;
using WebMoney.Services.Contracts.BasicTypes;
using WebMoney.Services.Contracts.BusinessObjects;
using WebMoney.Services.Utils;

namespace WebMoney.Services.BusinessObjects
{
    [Serializable]
    [XmlRoot("authenticationSettings")]
    public sealed class AuthenticationSettings : IAuthenticationSettings
    {
        private IRequestNumberSettings _requestNumberSettings;
        private IConnectionSettings _connectionSettings;
        private IProxySettings _proxySettings;

        [XmlAttribute("authenticationMethod")]
        public AuthenticationMethod AuthenticationMethod { get; set; }

        [XmlAttribute("identifier")]
        [DisplayFormat(DataFormatString = FormattingService.IdentifierTemplate)]
        public long Identifier { get; set; }

        [XmlAttribute("keeperKey")]
        public byte[] KeeperKey { get; set; }

        [XmlAttribute("certificateThumbprint")]
        public string CertificateThumbprint { get; set; }

        [XmlElement("requestNumberSettings")]
        public IRequestNumberSettings RequestNumberSettings
        {
            get => _requestNumberSettings;
            set => _requestNumberSettings = BusinessObjects.RequestNumberSettings.Create(value);
        }

        [XmlElement("connectionSettings")]
        public IConnectionSettings ConnectionSettings
        {
            // TODO [L] Перенести или убрать проверку IsRunningOnMono в ConnectionSettings
            get => ApplicationUtility.IsRunningOnMono ? null : _connectionSettings;
            set => _connectionSettings = BusinessObjects.ConnectionSettings.Create(value);
        }

        [XmlElement("proxySettings")]
        public IProxySettings ProxySettings
        {
            get => _proxySettings;
            set => _proxySettings = BusinessObjects.ProxySettings.Create(value);
        }

        internal AuthenticationSettings()
        {
        }

        public static AuthenticationSettings Create(IAuthenticationSettings contractObject)
        {
            if (null == contractObject)
                return null;

            var businessObject = contractObject as AuthenticationSettings;

            if (businessObject != null)
                return businessObject;

            return Mapper.Map<AuthenticationSettings>(contractObject);
        }

        public byte[] GetKeeperKey()
        {
            return KeeperKey;
        }
    }
}
