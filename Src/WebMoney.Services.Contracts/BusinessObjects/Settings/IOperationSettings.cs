using System.ComponentModel;
using System.Drawing;

namespace WebMoney.Services.Contracts.BusinessObjects
{
    [TypeConverter(typeof(ComplexObjectConverter))]
    public interface IOperationSettings
    {
        Color IncomeForeColor { get; set; }
        Color OutcomeForeColor { get; set; }
        Color IncomeChartColor { get; set; }
        Color OutcomeChartColor { get; set; }
        Color SelectionColor { get; set; }
        Color ProtectedColor { get; set; }
    }
}
