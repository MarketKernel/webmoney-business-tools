using System;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using WebMoney.Services.BusinessObjects;
using WebMoney.Services.Contracts;
using WebMoney.Services.Contracts.BasicTypes;
using WebMoney.Services.Contracts.BusinessObjects;

namespace WebMoney.Services.Tests.FakeServices
{
    internal sealed class AuthenticationService : IAuthenticationService
    {
        public long MasterIdentifier => 1;
        public AuthenticationMethod AuthenticationMethod => AuthenticationMethod.KeeperClassic;
        public bool HasPassword { get; }

        public bool HasConnectionSettings => true;

        public void SetPassword(string password, string freshPassword)
        {
            throw new NotImplementedException();
        }

        public void SetKeeperKey(byte[] keeperKey)
        {
            throw new NotImplementedException();
        }

        public string Sign(string message)
        {
            throw new NotImplementedException();
        }

        public X509Certificate2 GetCertificate()
        {
            throw new NotImplementedException();
        }

        public void SetCertificate(string certificateThumbprint)
        {
            throw new NotImplementedException();
        }

        public IRequestNumberSettings GetRequestNumberSettings()
        {
            throw new NotImplementedException();
        }

        public void SetRequestNumberSettings(IRequestNumberSettings requestNumberSettings)
        {
            throw new NotImplementedException();
        }

        public long GetRequestNumber(RequestNumberGenerationMethod method, long increment)
        {
            throw new NotImplementedException();
        }

        public IConnectionSettings GetConnectionSettings()
        {
            return new ConnectionSettings("Data Source=TestDB-v1.sdf;Persist Security Info=False;", "System.Data.SqlServerCe.4.0");
        }

        public void SetConnectionSettings(IConnectionSettings connectionSettings)
        {
            throw new NotImplementedException();
        }

        public WebProxy GetProxy()
        {
            throw new NotImplementedException();
        }

        public IProxySettings GeProxySettings()
        {
            throw new NotImplementedException();
        }

        public void SetProxySettings(IProxySettings proxySettings)
        {
            throw new NotImplementedException();
        }
    }
}
