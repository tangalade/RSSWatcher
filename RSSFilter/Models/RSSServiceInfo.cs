using RSSFilter.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RSSFilter.Models
{
    public class RSSServiceInfo
    {
        public int Id { get; set; }
        [Sortable]
        [Required]
        [Display(Name = "RSS Feed URL")]
        public string RSSURL { get; set; }
        [Sortable(true)]
        [Required]
        [StringLength(255)]
        [Display(Name = "RSS Feed Name")]
        public string Name { get; set; }
        [Sortable]
        [Display(Name = "Status")]
        public bool IsRunning { get; set; }
        [Display(Name = "Creation Date")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTimeOffset CreationDate { get; set; }
        [Sortable]
        [Display(Name = "Active Since")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTimeOffset LastStartDate { get; set; }
        [Display(Name = "Inactive Since")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTimeOffset LastStopDate { get; set; }
    }
}