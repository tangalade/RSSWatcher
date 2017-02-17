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
    public class ArtistController : ListController<Artist>
    {
        private ApplicationDbContext _context;
        public ArtistController()
        {
            _context = new ApplicationDbContext();
        }
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        public ActionResult Rate(int id, Rating rating)
        {
            var artist = _context.Artists.Where(a => a.Id == id).SingleOrDefault();
            if (artist != null)
            {
                artist.Rating = rating;
                _context.SaveChanges();
            }
            return Redirect(Request.UrlReferrer.ToString());
        }
        public async Task<ActionResult> Index()
        {
            processActionCookies();
            ListControl<Artist> control = new ListControl<Artist>();
            control.setFromCookies(Request.Cookies);

            var artists = _context.Artists;

            var listViewModel = await ListViewModel<Artist>.CreateAsync(artists, control);
            control.fillCookies(Response.Cookies);
            return View(listViewModel);
        }
    }
}