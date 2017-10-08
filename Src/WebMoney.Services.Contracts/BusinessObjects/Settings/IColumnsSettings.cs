using System.Collections.ObjectModel;

namespace WebMoney.Services.Contracts.BusinessObjects
{
    public interface IColumnsSettings
    {
        Collection<int> ColumnOrders { get; }
        Collection<int> ColumnWidths { get; }
    }
}
