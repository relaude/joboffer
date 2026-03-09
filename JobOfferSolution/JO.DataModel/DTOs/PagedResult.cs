using System;
using System.Collections.Generic;
using System.Text;

namespace JO.DataModel.DTOs
{
    public class PagedResult<T>
    {
        public List<T> Data { get; set; } = new();
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
