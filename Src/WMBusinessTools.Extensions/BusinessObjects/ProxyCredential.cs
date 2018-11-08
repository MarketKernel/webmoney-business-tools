using System;
using WebMoney.Services.Contracts.BusinessObjects;

namespace WMBusinessTools.Extensions.BusinessObjects
{
    internal sealed class ProxyCredential : IProxyCredential
    {
        private string _username;
        private string _password;

        public string Username
        {
            get => _username;
            set => _username = value ?? throw new ArgumentNullException(nameof(value));
        }

        public string Password
        {
            get => _password;
            set => _password = value ?? throw new ArgumentNullException(nameof(value));
        }

        public ProxyCredential(string username, string password)
        {
            _username = username ?? throw new ArgumentNullException(nameof(username));
            _password = password ?? throw new ArgumentNullException(nameof(password));
        }
    }
}
