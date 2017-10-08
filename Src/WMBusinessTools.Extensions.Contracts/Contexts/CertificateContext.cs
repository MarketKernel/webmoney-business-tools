using System;
using WebMoney.Services.Contracts.BusinessObjects;

namespace WMBusinessTools.Extensions.Contracts.Contexts
{
    public class CertificateContext : SessionContext
    {
        public ICertificate Certificate { get; }

        public CertificateContext(CertificateContext origin)
            : base(origin)
        {
            Certificate = origin.Certificate;
        }

        public CertificateContext(SessionContext baseContext, ICertificate certificate)
            : base(baseContext)
        {
            Certificate = certificate ?? throw new ArgumentNullException(nameof(certificate));
        }
    }
}
