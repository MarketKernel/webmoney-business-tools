﻿using System;
using System.Collections.Generic;
using WebMoney.Services.Contracts.BusinessObjects;

namespace WebMoney.Services.BusinessObjects
{
    internal sealed class PageOfT<TItem> : IPageOfT<TItem>
    {
        public IEnumerable<TItem> Items { get; }

        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }

        public PageOfT(IEnumerable<TItem> items, int currentPage, int totalPages)
        {
            Items = items ?? throw new ArgumentNullException(nameof(items));
            CurrentPage = currentPage;
            TotalPages = totalPages;
        }
    }
}
