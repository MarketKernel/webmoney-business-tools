using System.ComponentModel;

namespace WebMoney.Services.Contracts.BusinessObjects
{
    [TypeConverter(typeof(ComplexObjectConverter))]
    public interface IRequestSettings
    {
        byte TransferMaxRequestPeriod { get; set; }
        byte IncomingInvoiceMaxRequestPeriod { get; set; }
        byte OutgoingInvoiceMaxRequestPeriod { get; set; }
    }
}
