using System;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using WebMoney.Services.Contracts;
using WebMoney.Services.Contracts.BasicTypes;
using WebMoney.XmlInterfaces;
using WebMoney.XmlInterfaces.BasicObjects;

namespace WebMoney.Services.BusinessObjects
{
    internal sealed class ExtendedInitializer : Initializer
    {
        private static readonly object Anchor = new object();

        private readonly IAuthenticationService _authenticationService;

        private long _lastNumber;

        public override AuthorizationMode Mode
        {
            get
            {
                if (null == _authenticationService)
                    return base.Mode;

                switch (_authenticationService.AuthenticationMethod)
                {
                    case AuthenticationMethod.KeeperClassic:
                        return AuthorizationMode.Classic;
                    case AuthenticationMethod.KeeperLight:
                        return AuthorizationMode.Light;
                    default:
                        throw new InvalidOperationException("_authenticationService.AuthenticationMethod == " +
                                                            _authenticationService.AuthenticationMethod);
                }
            }
            protected set => throw new NotSupportedException();
        }

        public override WmId Id
        {
            get
            {
                if (null == _authenticationService)
                    return base.Id;

                return (WmId) _authenticationService.MasterIdentifier;
            }
            protected set => throw new NotSupportedException();
        }

        public override X509Certificate2 Certificate
        {
            get
            {
                if (null == _authenticationService)
                    throw new InvalidOperationException("null == _authenticationService");

                return _authenticationService.GetCertificate();
            }
            protected set => throw new NotSupportedException();
        }

        public override WebProxy Proxy
        {
            get
            {
                if (null == _authenticationService)
                    throw new InvalidOperationException("null == _authenticationService");

                return _authenticationService.GetProxy();
            }
            protected set => throw new NotSupportedException();
        }

        public ExtendedInitializer(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService ??
                                     throw new ArgumentNullException(nameof(authenticationService));
        }

        public override string Sign(string value)
        {
            if (null == value)
                throw new ArgumentNullException(nameof(value));

            return _authenticationService.Sign(value);
        }

        public override ulong GetRequestNumber()
        {
            var requestNumberSettings = _authenticationService.GetRequestNumberSettings();

            var method = requestNumberSettings.Method;
            var increment = requestNumberSettings.Increment;

            long requestNumber = _authenticationService.GetRequestNumber(method, increment);

            lock (Anchor)
            {
                if (requestNumber <= _lastNumber)
                    requestNumber = _lastNumber + 1;

                _lastNumber = requestNumber;
            }

            return (ulong) requestNumber;
        }
    }
}