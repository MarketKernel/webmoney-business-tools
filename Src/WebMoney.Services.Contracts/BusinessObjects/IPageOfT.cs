using System.Collections.Generic;

namespace WebMoney.Services.Contracts.BusinessObjects
{
    public interface IPageOfT<out TItem>
    {
        int CurrentPage { get; }
        int TotalPages { get; }

        IReadOnlyCollection<TItem> Items { get; }
    }
}
