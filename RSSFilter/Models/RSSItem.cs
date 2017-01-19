using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel.Syndication;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using RSSFilter.Common;

namespace RSSFilter.Models
{
    public class RSSItem
    {
        public int Id { get; set; }
        [Display(Name = "Parsed Correctly?")]
        public bool CorrectlyParsed { get; set; } = false;
        public ItemViewStatus viewStatus { get; set; } = ItemViewStatus.NEW;

        // Raw RSS fields
        [Required]
        [StringLength(255)]
        public string RSSId { get; set; }
        [Required]
        public string RSSTitle { get; set; }
        [Sortable(true,false)]
        [Filterable(typeof(DateFilterMethod))]
        [Display(Name = "Publish Date")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTimeOffset RSSPublishDate { get; set; }
        public string RSSURI { get; set; }

        // Parsed fields (specific to k2nblog RSSfeed)
        [Filterable("Name")]
        [Sortable("Name")]
        public ItemType ItemType { get; set; }
        public int ItemTypeId { get; set; }
        [Filterable("Name")]
        [Sortable("Name")]
        public Artist Artist { get; set; }
        public int ArtistId { get; set; }
        [Sortable]
        public string Title { get; set; } = "Unknown";


        [Obsolete("Only needed for Entity Framework", true)]
        private RSSItem() { }
        public RSSItem(SyndicationItem item)
        {
            RSSId = item.Id;
            RSSTitle = item.Title.Text;
            RSSPublishDate = item.PublishDate;
            if (item.Links.Count() > 0)
                RSSURI = item.Links[0].Uri.AbsoluteUri;
            parseRSSTitle(RSSTitle);
        }

        public void parseRSSTitle(string rssTitle)
        {
            var regex = new Regex(@"\[(.+)\]([^–]+)–(.*)");
            MatchCollection matches = regex.Matches(rssTitle);
            if (matches.Count == 1)
            {
                ItemType = new ItemType() { Name = matches[0].Groups[1].Value.Trim() };
                Artist = new Artist() { Name = matches[0].Groups[2].Value.Trim() };
                Title = matches[0].Groups[3].Value.Trim();
                CorrectlyParsed = true;
            }
            else
            {
                // failed to parse
                CorrectlyParsed = false;
            }
        }
    }
}