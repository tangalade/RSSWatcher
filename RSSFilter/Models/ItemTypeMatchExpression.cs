using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RSSFilter.Models
{
    public class ItemTypeMatchExpression
    {
        public int Id { get; set; }
        [Required]
        [StringLength(255)]
        public string Expression { get; set; }
        [Required]
        public ItemType ItemType { get; set; }
        [Required]
        public int ItemTypeId { get; set; }
        public bool Resolved { get; set; } = false;
    }
}