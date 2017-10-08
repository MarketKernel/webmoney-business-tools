using System.ComponentModel;
using System.Drawing;

namespace WebMoney.Services.Contracts.BusinessObjects
{
    [TypeConverter(typeof(ComplexObjectConverter))]
    public interface ITransferBundleSettings : IColumnsSettings
    {
        Color SelectionColor { get; set; }
        Color PendedColor { get; set; }
        Color ProcessedColor { get; set; }
        Color CompletedColor { get; set; }
    }
}