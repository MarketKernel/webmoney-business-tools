using System.ComponentModel;

namespace WebMoney.Services.Contracts.BusinessObjects
{
    [TypeConverter(typeof(ComplexObjectConverter))]
    public interface ITransferSettings : IColumnsSettings
    {
        bool SecondaryIdVisibility { get; set; }
        bool SourcePurseVisibility { get; set; }
        bool TargetPurseVisibility { get; set; }
        bool CommissionVisibility { get; set; }
        bool DescriptionVisibility { get; set; }
        bool TypeVisibility { get; set; }
        bool InvoiceIdVisibility { get; set; }
        bool OrderIdVisibility { get; set; }
        bool PaymentIdVisibility { get; set; }
        bool ProtectionPeriodVisibility { get; set; }
        bool PartnerIdentifierVisibility { get; set; }
        bool BalanceVisibility { get; set; }
        bool LockedVisibility { get; set; }
        bool CreationTimeVisibility { get; set; }
    }
}
