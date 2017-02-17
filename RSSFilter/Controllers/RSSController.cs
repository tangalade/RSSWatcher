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
    public class RSSController : ListController<RSSItem>
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
        public ActionResult Edit(int id)
        {
            // FIXME: do this
            var rssItems = _context.RSSItems.Include(r => r.Artist).Include(r => r.ItemType);
            var rssItem = rssItems.Where(r => r.Id == id).SingleOrDefault();
            return View();
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
            processActionCookies();
            ListControl<RSSItem> control = new ListControl<RSSItem>();
            control.setFromCookies(Request.Cookies);

            var rssItems = _context.RSSItems.Include(r => r.Artist).Include(r => r.ItemType);

            // exclude Archived items
            if (!all)
                rssItems = rssItems.Where(r => r.ViewStatus != ItemViewStatus.ARCHIVED);

            var listViewModel = await ListViewModel<RSSItem>.CreateAsync(rssItems, control);
            control.fillCookies(Response.Cookies);
            return View(listViewModel);
        }
    }
}