using RSSFilter.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RSSFilter.Models
{
    public class Artist
    {
        public static Artist UNKNOWN = new Artist() { Name = "Unkown" };
        public int Id { get; set; }
        [Sortable(true)]
        [Required]
        [StringLength(255)]
        [Display(Name = "Artist")]
        public string Name { get; set; }
        [Sortable]
        public Rating Rating { get; set; } = Rating.UNRATED;
    }
}