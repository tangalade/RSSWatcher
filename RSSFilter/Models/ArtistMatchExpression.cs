using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RSSFilter.Models
{
    public class ArtistMatchExpression
    {
        public int Id { get; set; }
        [Required]
        [StringLength(255)]
        public string Expression { get; set; }
        [Required]
        public Artist Artist { get; set; }
        [Required]
        public int ArtistId { get; set; }
        public bool Resolved { get; set; } = false;
    }
}