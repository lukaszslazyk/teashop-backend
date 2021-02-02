using System;
using System.Collections.Generic;

namespace Teashop.Backend.Application.Commons.Models
{
    public class PaginatedList<T>
    {
        public List<T> Items { get; set;  }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int PagesInTotal { get; set; }
        public int TotalCount { get; set; }

        public PaginatedList(List<T> items)
        {
            Items = items;
            PageIndex = 0;
            PageSize = items.Count;
            PagesInTotal = 1;
            TotalCount = items.Count;
        }

        public PaginatedList(List<T> items, int pageIndex, int pageSize, int totalCount)
        {
            Items = items;
            PageIndex = pageIndex;
            PageSize = pageSize > totalCount ? totalCount : pageSize;
            PagesInTotal = (int)Math.Ceiling(totalCount / (double)pageSize);
            TotalCount = totalCount;
        }
    }
}
