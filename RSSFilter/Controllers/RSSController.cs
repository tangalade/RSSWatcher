using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RSSFilter.Models;
using System.ServiceModel.Syndication;
using System.Xml;
using Hangfire;
using RSSFilter.Common;
using System.Data.Entity;
using RSSFilter.ViewModel;
using System.Threading.Tasks;

namespace RSSFilter.Controllers
{
    public class RSSController : Controller
    {
        private ApplicationDbContext _context;
        public RSSController()
        {
            _context = new ApplicationDbContext();
        }
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        public ActionResult Edit(int id)
        {
            var rssItems = _context.RSSItems.Include(r => r.Artist).Include(r => r.ItemType);
            var rssItem = rssItems.Where(r => r.Id == id).SingleOrDefault();
            return View();
        }
        public ActionResult PageSize(int pageSize)
        {
            ListControl<RSSItem> control = new ListControl<RSSItem>(Request.Cookies);
            control.pager.PageSize = pageSize;
            control.pager.reset();
            control.fillCookies(Response.Cookies);
            return Redirect(Request.UrlReferrer.ToString());
        }
        public ActionResult GoToPage(int pageNum)
        {
            ListControl<RSSItem> control = new ListControl<RSSItem>(Request.Cookies);
            if (pageNum < 1)
                pageNum = 1;
            control.pager.PageNum = pageNum;
            control.fillCookies(Response.Cookies);
            return Redirect(Request.UrlReferrer.ToString());
        }
        public ActionResult SortBy(string prop, bool ascending)
        {
            ListControl<RSSItem> control = new ListControl<RSSItem>(Request.Cookies);
            if (control.sortableFields.ContainsKey(prop))
            {
                control.sorter.Name = prop;
                control.sorter.Ascending = ascending;
            }
            control.fillCookies(Response.Cookies);
            return Redirect(Request.UrlReferrer.ToString());
        }
        public ActionResult FilterBy(string prop, string value)
        {
            ListControl<RSSItem> control = new ListControl<RSSItem>(Request.Cookies);
            if (control.filterableFields.ContainsKey(prop))
            {
                var filter = control.filterableFields[prop];
                filter.Value = value;
                control.filters.Add(filter);
            }
            control.fillCookies(Response.Cookies);
            return Redirect(Request.UrlReferrer.ToString());
        }
        public ActionResult RemoveFilter(string prop)
        {
            ListControl<RSSItem> control = new ListControl<RSSItem>(Request.Cookies);
            control.filters = control.filters.Where(f => f.Name != prop).ToList();
            control.fillCookies(Response.Cookies);
            return Redirect(Request.UrlReferrer.ToString());
        }
        public ActionResult RateArtist(int id, Rating rating)
        {
            var artist = _context.Artists.Where(a => a.Id == id).SingleOrDefault();
            if (artist != null)
            {
                artist.Rating = rating;
                _context.SaveChanges();
            }
            return Redirect(Request.UrlReferrer.ToString());
        }
        public ActionResult RateItemType(int id, Rating rating)
        {
            var itemType = _context.ItemTypes.Where(i => i.Id == id).SingleOrDefault();
            if (itemType != null)
            {
                itemType.Rating = rating;
                _context.SaveChanges();
            }
            return Redirect(Request.UrlReferrer.ToString());
        }
        public ActionResult ViewItem(int id)
        {
            var rssItems = _context.RSSItems.Include(r => r.Artist).Include(r => r.ItemType);
            var rssItem = rssItems.Where(r => r.Id == id).SingleOrDefault();
            if (rssItem != null)
            {
                rssItem.ViewStatus = ItemViewStatus.VIEWED;
                _context.SaveChanges();
                return Redirect(rssItem.RSSURI);
            }
            return Redirect(Request.UrlReferrer.ToString());
        }
        public ActionResult Archive(int id)
        {
            var rssItems = _context.RSSItems.Include(r => r.Artist).Include(r => r.ItemType);
            var rssItem = rssItems.Where(r => r.Id == id).SingleOrDefault();
            rssItem.ViewStatus = ItemViewStatus.ARCHIVED;
            _context.SaveChanges();
            return Redirect(Request.UrlReferrer.ToString());
        }
        public ActionResult MarkViewStatus(int[] ids, ItemViewStatus viewStatus)
        {
            var rssItems = _context.RSSItems.Include(r => r.Artist).Include(r => r.ItemType);
            for (int i = 0; i < ids.Length; i++)
            {
                var id = ids[i];
                var rssItem = rssItems.Where(r => r.Id == id).SingleOrDefault();
                if (rssItem != null)
                    rssItem.ViewStatus = viewStatus;
            }
            _context.SaveChanges();
            return Redirect(Request.UrlReferrer.ToString());
        }
        public async Task<ActionResult> Index(bool all = false)
        {
            // FIXME: what if came from Artist/ItemTypes?
            // need to have the cookies cleared out somehow, or ignored, or replaced
            ListControl<RSSItem> control = new ListControl<RSSItem>(Request.Cookies);
            var rssItems = _context.RSSItems.Include(r => r.Artist).Include(r => r.ItemType);

            // exclude Archived items
            if (!all)
                rssItems = rssItems.Where(r => r.ViewStatus != ItemViewStatus.ARCHIVED);

            control.fillCookies(Response.Cookies);
            return View(await ListViewModel<RSSItem>.CreateAsync(rssItems, control));
        }
        public async Task<ActionResult> Artists()
        {
            ListControl<Artist> control = new ListControl<Artist>(Request.Cookies);
            var artists = _context.Artists;

            control.fillCookies(Response.Cookies);
            return View(await ListViewModel<Artist>.CreateAsync(artists, control));
        }
        public async Task<ActionResult> ItemTypes()
        {
            ListControl<ItemType> control = new ListControl<ItemType>(Request.Cookies);
            var itemTypes = _context.ItemTypes;

            control.fillCookies(Response.Cookies);
            return View(await ListViewModel<ItemType>.CreateAsync(itemTypes, control));
        }

        //public ActionResult MarkNew(ListControl<RSSItem> control, int[] ids)
        //{
        //    var rssItems = _context.RSSItems.Include(r => r.Artist).Include(r => r.ItemType);

        //    for (int i=0; i<ids.Length; i++)
        //    {
        //        var id = ids[i];
        //        var rssItem = rssItems.Where(r => r.Id == id).SingleOrDefault();
        //        rssItem.ViewStatus = ItemViewStatus.NEW;
        //    }
        //    _context.SaveChanges();

        //    return RedirectToAction("Index", "RSS", new { control = control });
        //}

        //public ActionResult MarkViewed(ListControl<RSSItem> control, int[] ids)
        //{
        //    var rssItems = _context.RSSItems.Include(r => r.Artist).Include(r => r.ItemType);

        //    for (int i = 0; i < ids.Length; i++)
        //    {
        //        var id = ids[i];
        //        var rssItem = rssItems.Where(r => r.Id == id).SingleOrDefault();
        //        rssItem.ViewStatus = ItemViewStatus.VIEWED;
        //    }
        //    _context.SaveChanges();

        //    return RedirectToAction("Index", "RSS", new { control = control });
        //}

        //public ActionResult Archive(ListControl<RSSItem> control, int[] ids)
        //{
        //    var rssItems = _context.RSSItems.Include(r => r.Artist).Include(r => r.ItemType);

        //    for (int i = 0; i < ids.Length; i++)
        //    {
        //        var id = ids[i];
        //        var rssItem = rssItems.Where(r => r.Id == id).SingleOrDefault();
        //        rssItem.ViewStatus = ItemViewStatus.ARCHIVED;
        //    }
        //    _context.SaveChanges();

        //    return RedirectToAction("Index", "RSS", new { control = control });
        //}


        //public ActionResult ViewItem(ListControl<RSSItem> control, int id)
        //{
        //    var rssItems = _context.RSSItems.Include(r => r.Artist).Include(r => r.ItemType);
        //    var rssItem = rssItems.Where(r => r.Id == id).SingleOrDefault();

        //    if (rssItem != null)
        //    {
        //        rssItem.ViewStatus = ItemViewStatus.VIEWED;
        //        _context.SaveChanges();
        //        return Redirect(rssItem.RSSURI);
        //    }

        //    return RedirectToAction("Index", "RSS", new { control = control });
        //}

        //public ActionResult RateArtist(ListControl<RSSItem> control, int id, Rating rating)
        //{
        //    var artist = _context.Artists.Where(a => a.Id == id).SingleOrDefault();
        //    if (artist != null)
        //    {
        //        artist.Rating = rating;
        //        _context.SaveChanges();
        //    }

        //    return RedirectToAction("Index", "RSS", new { control = control });
        //}

        //public ActionResult RateItemType(ListControl<RSSItem> control, int id, Rating rating)
        //{
        //    var itemType = _context.ItemTypes.Where(a => a.Id == id).SingleOrDefault();
        //    if (itemType != null)
        //    {
        //        itemType.Rating = rating;
        //        _context.SaveChanges();
        //    }

        //    return RedirectToAction("Index", "RSS", new { control = control });
        //}

        // old version using JS to submit forms
        //public async Task<ActionResult> Index(ListControl<RSSItem> control)
        //{
        //    var rssItems = _context.RSSItems.Include(r => r.Artist).Include(r => r.ItemType);

        //    // exclude Archived items
        //    rssItems = rssItems.Where(r => r.ViewStatus != ItemViewStatus.ARCHIVED);

        //    // create blank controls if needed
        //    if (control == null)
        //        control = new ListControl<RSSItem>();

        //    return View(await ListViewModel<RSSItem>.CreateAsync(rssItems, control));
        //}
    }
}