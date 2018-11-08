using System;

namespace WebMoney.Services.Contracts.BasicTypes
{
    [Flags]
    public enum CurrencyCapabilities
    {
        None = 0,
        Actual = 0x01,
        Invoice = 0x02,
        TransferByInvoice = 0x04,
        Transfer = 0x08,
        Debit = 0x10,
        Credit = 0x20,
        CreatePurse = 0x40
    }
}
