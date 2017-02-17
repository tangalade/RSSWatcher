using RSSFilter.Common;
using RSSFilter.Models;
using RSSFilter.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace RSSFilter.Controllers
{
    public class ServiceController : ListController<RSSServiceInfo>
    {
        private static string k2nblogURL { get; } = "http://feeds.feedburner.com/k2nfeed";
        private static string k2nblogName { get; } = "K2NBlog";

        private ApplicationDbContext _context;
        // GET: Service
        public ServiceController()
        {
            _context = new ApplicationDbContext();
        }
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
        public ActionResult Delete(int id)
        {
            var rssServiceInfos = _context.RSSServiceInfoes;
            var rssServiceInfo = rssServiceInfos.Where(r => r.Id == id).SingleOrDefault();
            RSSService rssService = new RSSService() { rssURL = rssServiceInfo.RSSURL };
            if (rssServiceInfo != null)
            {
                if (rssServiceInfo.IsRunning)
                {
                    rssService.stop();
                }
                _context.RSSServiceInfoes.Remove(rssServiceInfo);
                _context.SaveChanges();
            }
            return Redirect(Request.UrlReferrer.ToString());
        }
        public ActionResult Start(int id)
        {
            var rssServiceInfos = _context.RSSServiceInfoes;
            var rssServiceInfo = rssServiceInfos.Where(r => r.Id == id).SingleOrDefault();
            // store HF job id in db with rssURL (probably just use the rssURL)
            // add note to View to stop and start the service if it says Active, because the state is not known for certain
            RSSService rssService = new RSSService() { rssURL = rssServiceInfo.RSSURL };
            // use rssService.JobId to check DB
            // if job id exists, check if it is running
            if (rssServiceInfo != null)
            {
                // if it says it is running, stop and restart the service and update LastStartDate
                if (rssServiceInfo.IsRunning)
                {
                    rssService.stop();
                    rssService.start();
                    rssServiceInfo.LastStartDate = DateTimeOffset.Now;
                    rssServiceInfo.LastStopDate = DateTimeOffset.Now;
                }
                // else start the service and update the db with IsRunning and LastStartDate
                else
                {
                    rssService.start();
                    rssServiceInfo.IsRunning = true;
                    rssServiceInfo.LastStartDate = DateTimeOffset.Now;
                }
            }
            _context.SaveChanges();
            return Redirect(Request.UrlReferrer.ToString());
        }
        public ActionResult Stop(int id)
        {
            var rssServiceInfos = _context.RSSServiceInfoes;
            var rssServiceInfo = rssServiceInfos.Where(r => r.Id == id).SingleOrDefault();
            // store HF job id in db with rssURL (probably just use the rssURL)
            // add note to View to stop and start the service if it says Active, because the state is not known for certain
            RSSService rssService = new RSSService() { rssURL = rssServiceInfo.RSSURL };
            // use rssService.JobId to check DB
            // if job id exists, check if it is running
            if (rssServiceInfo != null)
            {
                // if it is running, stop the service and mark it as stopped in the db
                if (rssServiceInfo.IsRunning)
                {
                    rssService.stop();
                    rssServiceInfo.IsRunning = false;
                    rssServiceInfo.LastStopDate = DateTimeOffset.Now;
                }
                // else do nothing
                else
                {
                }
            }
            _context.SaveChanges();
            return Redirect(Request.UrlReferrer.ToString());
        }
        public ActionResult Create(RSSServiceInfo rssServiceInfo)
        {
            rssServiceInfo.IsRunning = false;
            rssServiceInfo.CreationDate = DateTimeOffset.Now;
            rssServiceInfo.LastStartDate = DateTimeOffset.Now;
            rssServiceInfo.LastStopDate = DateTimeOffset.Now;
            _context.RSSServiceInfoes.Add(rssServiceInfo);
            _context.SaveChanges();

            return Redirect(Request.UrlReferrer.ToString());
        }
        public void processActionCookies<T>(ListControl<T> control, string action)
        {
            // if came from a different Action, clear the control cookies
            if (Request.Cookies[actionCookieToken] != null)
                if (Request.Cookies[actionCookieToken].Value != action)
                    control.clearCookies(Request.Cookies);
            Response.Cookies[actionCookieToken].Value = action;
        }
        public async Task<ActionResult> Index(bool all = false)
        {
            ListControl<RSSServiceInfo> control = new ListControl<RSSServiceInfo>();
            processActionCookies(control, "Index");
            control.setFromCookies(Request.Cookies);

            var rssServiceInfos = _context.RSSServiceInfoes;

            var listViewModel = await ListViewModel<RSSServiceInfo>.CreateAsync(rssServiceInfos, control);
            control.fillCookies(Response.Cookies);
            return View(listViewModel);
        }
    }
}