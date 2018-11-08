using System.ComponentModel;
using System.Drawing;

namespace WebMoney.Services.Contracts.BusinessObjects
{
    [TypeConverter(typeof(ComplexObjectConverter))]
    public interface IPreparedTransferSettings : IColumnsSettings
    {
        Color FailedColor { get; set; }
        Color SelectionColor { get; set; }
        Color PendedColor { get; set; }
        Color ProcessedColor { get; set; }
        Color InterruptedColor { get; set; }
        Color CompletedColor { get; set; }

        bool PrimaryIdVisibility { get; set; }
        bool SecondaryIdVisibility { get; set; }
        bool PaymentIdVisibility { get; set; }
        bool SourcePurseVisibility { get; set; }
        bool TargetPurseVisibility { get; set; }
        bool DescriptionVisibility { get; set; }
        bool ForceVisibility { get; set; }
        bool CreationTimeVisibility { get; set; }
        bool TransferCreationTimeVisibility { get; set; }
    }
}
