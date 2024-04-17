using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers.ViewModels
{
    public class PaginationVM<T>
    {
        public IEnumerable<T> Items { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }

        public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);

        public bool HasPreviousPage => PageIndex > 1;

        public bool HasNextPage => PageIndex < TotalPages;

        public PaginationVM(IEnumerable<T> items, int pageIndex, int pageSize, int totalItems)
        {
            Items = items;
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalItems = totalItems;
        }
    }

}
