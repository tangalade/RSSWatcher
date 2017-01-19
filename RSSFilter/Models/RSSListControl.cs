using RSSFilter.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RSSFilter.Models
{
    public class RSSListControl
    {
        public RSSListFilters filters { get; set; } = new RSSListFilters();
        public SortingToken sorter { get; set; } = new SortingToken() { Name = "RSSPublishDate", Ascending = false };
        public Paging pager { get; set; } = new Paging() { PageSize = 10 };
    }
}