using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RSSFilter.Models
{
    public class RSSListFilters
    {
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTimeOffset RSSPublishDateFilter { get; set; }
        public bool FilterByRSSPublishDate { get; set; } = false;
        public string ArtistFilter { get; set; }
        public bool FilterByArtist { get; set; } = false;
        public string ItemTypeFilter { get; set; }
        public bool FilterByItemType { get; set; } = false;
    }
}