using System.ComponentModel;
using System.Drawing;

namespace WebMoney.Services.Contracts.BusinessObjects
{
    [TypeConverter(typeof(ComplexObjectConverter))]
    public interface IContractSettings : IColumnsSettings
    {
        Color PublicForeColor { get; set; }
        Color SelectionColor { get; set; }
        Color SignedColor { get; set; }

        bool TextVisibility { get; set; }
        bool AcceptedCountVisibility { get; set; }
        bool AccessCountVisibility { get; set; }
    }
}
