using RSSFilter.Common;
using RSSFilter.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace RSSFilter.ViewModel
{
    public class RSSListViewModel
    {
        public List<RSSItem> rssItems { get; set; }
        public RSSListControl control { get; set; }

        public static async Task<RSSListViewModel> CreateAsync(IQueryable<RSSItem> source, RSSListControl control)
        {
            var rssItems = source;
            // apply any filters to rssItems
            if (control.filters.FilterByRSSPublishDate)
                rssItems = rssItems.Where(r => r.RSSPublishDate.Year == control.filters.RSSPublishDateFilter.Year
                    && r.RSSPublishDate.Month == control.filters.RSSPublishDateFilter.Month
                    && r.RSSPublishDate.Day == control.filters.RSSPublishDateFilter.Day);
            if (control.filters.FilterByItemType)
                rssItems = rssItems.Where(r => r.ItemType.Name == control.filters.ItemTypeFilter);
            if (control.filters.FilterByArtist)
                rssItems = rssItems.Where(r => r.Artist.Name == control.filters.ArtistFilter);

            // apply any sorters to rssItems
            switch (control.sorter.Name)
            {
                case "RSSPublishDate":
                    if (control.sorter.Ascending)
                        rssItems = rssItems.OrderBy(i => i.RSSPublishDate);
                    else
                        rssItems = rssItems.OrderByDescending(i => i.RSSPublishDate);
                    break;
                case "ItemType":
                    if (control.sorter.Ascending)
                        rssItems = rssItems.OrderBy(i => i.ItemType.Name);
                    else
                        rssItems = rssItems.OrderByDescending(i => i.ItemType.Name);
                    break;
                case "Artist":
                    if (control.sorter.Ascending)
                        rssItems = rssItems.OrderBy(i => i.Artist.Name);
                    else
                        rssItems = rssItems.OrderByDescending(i => i.Artist.Name);
                    break;
                case "Title":
                    if (control.sorter.Ascending)
                        rssItems = rssItems.OrderBy(i => i.Title);
                    else
                        rssItems = rssItems.OrderByDescending(i => i.Title);
                    break;
            }

            var count = await rssItems.CountAsync();
            control.pager.NumPages = (int)Math.Ceiling((double)count/control.pager.PageSize);
            var rssItemsPage = await rssItems.Skip((control.pager.PageNum - 1) * control.pager.PageSize).Take(control.pager.PageSize).ToListAsync();

            return new RSSListViewModel() { rssItems = rssItemsPage, control = control };
        }
    }
}