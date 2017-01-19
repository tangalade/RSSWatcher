using RSSFilter.Common;
using RSSFilter.Models;
using RSSFilter.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RSSFilter.Controllers
{
    public class ServiceController : Controller
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
        public ActionResult Index(string sortOrder)
        {
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["URLSortParm"] = sortOrder == "URL" ? "url_desc" : "URL";
            ViewData["LastStartDateSortParm"] = sortOrder == "LastStartDate" ? "laststartdate_desc" : "LastStartDate";
            ViewData["StatusSortParm"] = sortOrder == "Status" ? "status_desc" : "Status";
            IQueryable<RSSServiceInfo> rssServiceInfoes = _context.RSSServiceInfoes;

            switch (sortOrder)
            {
                case "name_desc":
                    rssServiceInfoes = rssServiceInfoes.OrderByDescending(s => s.Name);
                    break;
                case "URL":
                    rssServiceInfoes = rssServiceInfoes.OrderBy(s => s.RSSURL);
                    break;
                case "url_desc":
                    rssServiceInfoes = rssServiceInfoes.OrderByDescending(s => s.RSSURL);
                    break;
                case "LastStartDate":
                    rssServiceInfoes = rssServiceInfoes.OrderBy(s => s.LastStartDate);
                    break;
                case "laststartdate_desc":
                    rssServiceInfoes = rssServiceInfoes.OrderByDescending(s => s.LastStartDate);
                    break;
                case "Status":
                    rssServiceInfoes = rssServiceInfoes.OrderByDescending(s => s.IsRunning);
                    break;
                case "status_desc":
                    rssServiceInfoes = rssServiceInfoes.OrderBy(s => s.IsRunning);
                    break;
                default:
                    rssServiceInfoes = rssServiceInfoes.OrderBy(s => s.Name);
                    break;
            }
            var formViewModel = new RSSServiceFormViewModel()
            {
                RSSServiceInfoes = rssServiceInfoes
            };
            return View(formViewModel);
        }

        public ActionResult Submit(RSSServiceInfo rssServiceInfo, string command)
        {
            if (!ModelState.IsValid)
            {
                var rssServiceInfoes = _context.RSSServiceInfoes;
                var formViewModel = new RSSServiceFormViewModel()
                {
                    RSSServiceInfoes = rssServiceInfoes,
                    RSSServiceInfo = rssServiceInfo
                };
                return View("Index", formViewModel);
            }
            switch (command) {
                case "Create":
                    return RedirectToAction("Create", "Service", rssServiceInfo);
                case "Delete":
                    return RedirectToAction("Delete", "Service", rssServiceInfo);
                case "Start":
                    return RedirectToAction("Start", "Service", rssServiceInfo);
                case "Stop":
                    return RedirectToAction("Stop", "Service", rssServiceInfo);
                default:
                    return RedirectToAction("Index", "Service");
            }
        }
        public ActionResult Create(RSSServiceInfo rssServiceInfo)
        {
            rssServiceInfo.IsRunning = false;
            rssServiceInfo.CreationDate = DateTimeOffset.Now;
            rssServiceInfo.LastStartDate = DateTimeOffset.Now;
            rssServiceInfo.LastStopDate = DateTimeOffset.Now;
            _context.RSSServiceInfoes.Add(rssServiceInfo);
            _context.SaveChanges();

            return RedirectToAction("Index", "Service");
        }

        public ActionResult Delete(RSSServiceInfo rssServiceInfo)
        {
            RSSService rssService = new RSSService() { rssURL = rssServiceInfo.RSSURL };
            var rssServiceInfoDb = _context.RSSServiceInfoes.SingleOrDefault(r => r.RSSURL == rssServiceInfo.RSSURL);
            if (rssServiceInfoDb != null)
            {
                if (rssServiceInfoDb.IsRunning)
                {
                    rssService.stop();
                }
                _context.RSSServiceInfoes.Remove(rssServiceInfoDb);
                _context.SaveChanges();
            }

            return RedirectToAction("Index", "Service");
        }
        public ActionResult Start(RSSServiceInfo rssServiceInfo)
        {
            // store HF job id in db with rssURL (probably just use the rssURL)
            // add note to View to stop and start the service if it says Active, because the state is not known for certain
            RSSService rssService = new RSSService() { rssURL = rssServiceInfo.RSSURL };
            var rssServiceInfoDb = _context.RSSServiceInfoes.SingleOrDefault(r => r.RSSURL == rssServiceInfo.RSSURL);
            // use rssService.JobId to check DB
            // if job id exists, check if it is running
            if (rssServiceInfoDb != null)
            {
                // if it says it is running, stop and restart the service and update LastStartDate
                if (rssServiceInfoDb.IsRunning)
                {
                    rssService.stop();
                    rssService.start();
                    rssServiceInfoDb.LastStartDate = DateTimeOffset.Now;
                    rssServiceInfoDb.LastStopDate = DateTimeOffset.Now;
                }
                // else start the service and update the db with IsRunning and LastStartDate
                else
                {
                    rssService.start();
                    rssServiceInfoDb.IsRunning = true;
                    rssServiceInfoDb.LastStartDate = DateTimeOffset.Now;
                }
            }
            // else start the service, and add to db
            else
            {
                rssService.start();
                rssServiceInfoDb = new RSSServiceInfo()
                {
                    RSSURL = rssService.rssURL,
                    Name = rssServiceInfo.Name,
                    IsRunning = true,
                    CreationDate = DateTimeOffset.Now,
                    LastStartDate = DateTimeOffset.Now,
                    LastStopDate = DateTimeOffset.Now
                };
                _context.RSSServiceInfoes.Add(rssServiceInfoDb);
            }
            _context.SaveChanges();
            return RedirectToAction("Index", "Service");
        }

        public ActionResult Stop(RSSServiceInfo rssServiceInfo)
        {
            // store HF job id in db with rssURL (probably just use the rssURL)
            // add note to View to stop and start the service if it says Active, because the state is not known for certain
            RSSService rssService = new RSSService() { rssURL = rssServiceInfo.RSSURL };
            var rssServiceInfoDb = _context.RSSServiceInfoes.SingleOrDefault(r => r.RSSURL == rssServiceInfo.RSSURL);
            // use rssService.JobId to check DB
            // if job id exists, check if it is running
            if (rssServiceInfoDb != null)
            {
                // if it is running, stop the service and mark it as stopped in the db
                if (rssServiceInfoDb.IsRunning)
                {
                    rssService.stop();
                    rssServiceInfoDb.IsRunning = false;
                    rssServiceInfoDb.LastStopDate = DateTimeOffset.Now;
                }
                // else do nothing
                else
                {
                }
            }
            // else do nothing
            else
            {
            }
            _context.SaveChanges();
            return RedirectToAction("Index", "Service");
        }

    }
}