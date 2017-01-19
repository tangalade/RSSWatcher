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

        public ActionResult Edit(int itemId)
        {
            var rssItems = _context.RSSItems.Include(r => r.Artist).Include(r => r.ItemType);
            var rssItem = rssItems.Where(r => r.Id == itemId).SingleOrDefault();
            return View();
        }

        public ActionResult MarkNew(ListControl<RSSItem> control, int[] itemIds)
        {
            var rssItems = _context.RSSItems.Include(r => r.Artist).Include(r => r.ItemType);

            for (int i=0; i<itemIds.Length; i++)
            {
                var rssItem = rssItems.Where(r => r.Id == itemIds[i]).SingleOrDefault();
                rssItem.viewStatus = ItemViewStatus.NEW;
            }
            _context.SaveChanges();

            return RedirectToAction("Index", "RSS", new { control = control });
        }

        public ActionResult MarkViewed(ListControl<RSSItem> control, int[] itemIds)
        {
            var rssItems = _context.RSSItems.Include(r => r.Artist).Include(r => r.ItemType);

            for (int i = 0; i < itemIds.Length; i++)
            {
                var rssItem = rssItems.Where(r => r.Id == itemIds[i]).SingleOrDefault();
                rssItem.viewStatus = ItemViewStatus.VIEWED;
            }
            _context.SaveChanges();

            return RedirectToAction("Index", "RSS", new { control = control });
        }

        public ActionResult Archive(ListControl<RSSItem> control, int[] itemIds)
        {
            var rssItems = _context.RSSItems.Include(r => r.Artist).Include(r => r.ItemType);

            for (int i = 0; i < itemIds.Length; i++)
            {
                var rssItem = rssItems.Where(r => r.Id == itemIds[i]).SingleOrDefault();
                rssItem.viewStatus = ItemViewStatus.ARCHIVED;
            }
            _context.SaveChanges();

            return RedirectToAction("Index", "RSS", new { control = control });
        }


        public ActionResult ReadItem(ListControl<RSSItem> control, int itemId)
        {
            var rssItems = _context.RSSItems.Include(r => r.Artist).Include(r => r.ItemType);
            var rssItem = rssItems.Where(r => r.Id == itemId).SingleOrDefault();

            if (rssItem != null)
            {
                rssItem.viewStatus = ItemViewStatus.VIEWED;
                _context.SaveChanges();
                return Redirect(rssItem.RSSURI);
            }

            return RedirectToAction("Index", "RSS", new { control = control });
        }

        public ActionResult RateArtist(ListControl<RSSItem> control, int artistId, Rating rating)
        {
            var artist = _context.Artists.Where(a => a.Id == artistId).SingleOrDefault();
            if (artist != null)
            {
                artist.Rating = rating;
                _context.SaveChanges();
            }

            return RedirectToAction("Index", "RSS", new { control = control });
        }

        public ActionResult RateItemType(ListControl<RSSItem> control, int itemId, Rating rating)
        {
            var itemType = _context.ItemTypes.Where(a => a.Id == itemId).SingleOrDefault();
            if (itemType != null)
            {
                itemType.Rating = rating;
                _context.SaveChanges();
            }

            return RedirectToAction("Index", "RSS", new { control = control });
        }

        public async Task<ActionResult> Index(ListControl<RSSItem> control)
        {
            // FIXME: could get bad once the db increases in size
            // get all rssItems
            var rssItems = _context.RSSItems.Include(r => r.Artist).Include(r => r.ItemType);

            // create blank controls if needed
            if (control == null)
                control = new ListControl<RSSItem>();

            return View(await ListViewModel<RSSItem>.CreateAsync(rssItems, control));
        }
        //public async Task<ActionResult> Index(RSSListControl control)
        //{
        //    // FIXME: could get bad once the db increases in size
        //    // get all rssItems
        //    var rssItems = _context.RSSItems.Include(r => r.Artist).Include(r => r.ItemType);

        //    // create blank controls if needed
        //    if (control == null)
        //        control = new RSSListControl();

        //    ListControl<RSSItem> listControl = new ListControl<RSSItem>();
        //    listControl.filters[0] = listControl.filterableFields["Artist"];
        //    listControl.filters[0].Value = "Cosmic Girls";
        //    listControl.sorter = listControl.sortableFields["Artist"];
        //    ListViewModel<RSSItem> listViewModel = await ListViewModel<RSSItem>.CreateAsync(rssItems, listControl);

        //    return View(await RSSListViewModel.CreateAsync(rssItems, control));
        //}
    }
}