using System.ComponentModel;

namespace WebMoney.Services.Contracts.BusinessObjects
{
    [TypeConverter(typeof(ComplexObjectConverter))]
    public interface IOutgoingInvoiceSettings : IColumnsSettings
    {
        bool SecondaryIdVisibility { get; set; }
        bool OrderIdVisibility { get; set; }
        bool SourcePurseVisibility { get; set; }
        bool TargetPurseVisibility { get; set; }
        bool DescriptionVisibility { get; set; }
        bool AddressVisibility { get; set; }
        bool ProtectionPeriodVisibility { get; set; }
        bool ExpirationPeriodVisibility { get; set; }
        bool TransferPrimaryIdVisibility { get; set; }
        bool СreationTimeVisibility { get; set; }
    }
}
