using System;

namespace WebMoney.Services.Contracts.BasicTypes
{
    [Flags]
    public enum CurrencyCapabilities
    {
        None = 0,
        Actual = 0x01,
        Invoice = 0x02,
        Transfer = 0x04,
        CreatePurse = 0x08
    }
}
