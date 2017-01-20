using RSSFilter.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RSSFilter.Models
{
    public class ItemType
    {
        public static ItemType UNKNOWN = new ItemType() { Name = "Unkown" };
        public int Id { get; set; }
        [Sortable(true)]
        [Required]
        [StringLength(255)]
        [Display(Name = "Type")]
        public string Name { get; set; }
        [Sortable]
        public Rating Rating { get; set; } = Rating.UNRATED;
    }
}