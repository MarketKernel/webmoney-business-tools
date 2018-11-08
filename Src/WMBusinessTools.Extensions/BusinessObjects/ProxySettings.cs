using System;
using WebMoney.Services.Contracts.BusinessObjects;

namespace WMBusinessTools.Extensions.BusinessObjects
{
    internal sealed class ProxySettings : IProxySettings
    {
        private string _host;

        public string Host
        {
            get => _host;
            set => _host = value ?? throw new ArgumentNullException(nameof(value));
        }

        public int Port { get; set; }
        public IProxyCredential Credential { get; set; }

        public ProxySettings(string host, int port)
        {
            _host = host ?? throw new ArgumentNullException(nameof(host));
            Port = port;
        }
    }
}
