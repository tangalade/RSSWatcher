using Hangfire;
using RSSFilter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Web;
using System.Xml;

namespace RSSFilter.Common
{
    public class RSSService
    {
        private ApplicationDbContext _context;
        public string rssURL { get; set; }
        public RSSService()
        {
            _context = new ApplicationDbContext();
        }
        ~RSSService()
        {
            _context.Dispose();
        }

        public IEnumerable<SyndicationItem> getRssItems(string rssURL)
        {
            var syndFeed = XmlReader.Create(rssURL);
            var syndItems = SyndicationFeed.Load(syndFeed);
            syndFeed.Close();
            return syndItems.Items;
        }

        public void GetAndUpdateRssItems(string rssURL)
        {
            IEnumerable<SyndicationItem> syndItems = getRssItems(rssURL);
            List<RSSItem> rssItems = new List<RSSItem>();
            foreach (SyndicationItem item in syndItems)
            {
                rssItems.Add(new RSSItem(item));
            }
            foreach (RSSItem item in rssItems)
            {
                // FIXME: for now only do lookups if item correctly parses
                if (item.CorrectlyParsed)
                {
                    // do nothing if item already exists in db
                    if (_context.RSSItems.Count(i => i.RSSId == item.RSSId) > 0)
                    continue;

                    // lookup item.ItemType in ItemType table
                    var matchingItemType = _context.ItemTypes.SingleOrDefault(a => a.Name == item.ItemType.Name);
                    // if no match, add it to ItemType table
                    if (matchingItemType == null)
                        item.ItemType = new ItemType() { Name = item.ItemType.Name };
                    else
                        item.ItemType = matchingItemType;

                    // lookup item.Artist in Artist table
                    var matchingArtist = _context.Artists.SingleOrDefault(a => a.Name == item.Artist.Name);
                    // if no match, add it to Artist table
                    if (matchingArtist == null)
                        item.Artist = new Artist() { Name = item.Artist.Name };
                    else
                        item.Artist = matchingArtist;

                    _context.RSSItems.Add(item);
                    _context.SaveChanges();
                }
            }
        }

        public void start()
        {
            // FIXME: throw exception if null
            if (rssURL != null)
                RecurringJob.AddOrUpdate(rssURL, () => GetAndUpdateRssItems(rssURL), Cron.Minutely);
        }

        public void stop()
        {
            // FIXME: throw exception if null
            if (rssURL != null)
                RecurringJob.RemoveIfExists(rssURL);
        }
    }
}