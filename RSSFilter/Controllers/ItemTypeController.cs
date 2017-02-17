using RSSFilter.Common;
using RSSFilter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace RSSFilter.Controllers
{
    public class ItemTypeController : ListController<ItemType>
    {
        private ApplicationDbContext _context;
        public ItemTypeController()
        {
            _context = new ApplicationDbContext();
        }
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        public ActionResult Rate(int id, Rating rating)
        {
            var itemType = _context.ItemTypes.Where(i => i.Id == id).SingleOrDefault();
            if (itemType != null)
            {
                itemType.Rating = rating;
                _context.SaveChanges();
            }
            return Redirect(Request.UrlReferrer.ToString());
        }
        public async Task<ActionResult> Index()
        {
            processActionCookies();
            ListControl<ItemType> control = new ListControl<ItemType>();
            control.setFromCookies(Request.Cookies);

            var itemTypes = _context.ItemTypes;

            var listViewModel = await ListViewModel<ItemType>.CreateAsync(itemTypes, control);
            control.fillCookies(Response.Cookies);
            return View(listViewModel);
        }

    }
}