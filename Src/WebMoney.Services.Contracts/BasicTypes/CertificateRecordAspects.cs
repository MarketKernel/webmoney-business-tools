using System;

namespace WebMoney.Services.Contracts.BasicTypes
{
    [Flags]
    public enum CertificateRecordAspects
    {
        None = 0,
        Protected = 1,
        Verified = 2
    }
}