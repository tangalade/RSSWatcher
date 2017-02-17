using RSSFilter.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RSSFilter.Controllers
{
    public class ListController<T> : Controller
    {
        public const string actionCookieToken = "ListControlAction";
        public ActionResult PageSize(int pageSize)
        {
            ListControl<T> control = new ListControl<T>(Request.Cookies);
            control.pager.PageSize = pageSize;
            control.pager.reset();
            control.fillCookies(Response.Cookies);
            return Redirect(Request.UrlReferrer.ToString());
        }
        public ActionResult GoToPage(int pageNum)
        {
            ListControl<T> control = new ListControl<T>(Request.Cookies);
            if (pageNum < 1)
                pageNum = 1;
            control.pager.PageNum = pageNum;
            control.fillCookies(Response.Cookies);
            return Redirect(Request.UrlReferrer.ToString());
        }
        public ActionResult SortBy(string prop, bool ascending)
        {
            // could be called to sort various types, not just T
            // need to get some kind of non-generic version of ListControl
            //   to be able to modify sorting, paging, and filtering properties
            //   there won't be any checking of the non-generic sorting, paging, and filtering changes
            ListControl<T> control = new ListControl<T>(Request.Cookies);
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
            ListControl<T> control = new ListControl<T>(Request.Cookies);
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
            ListControl<T> control = new ListControl<T>(Request.Cookies);
            control.filters = control.filters.Where(f => f.Name != prop).ToList();
            control.fillCookies(Response.Cookies);
            return Redirect(Request.UrlReferrer.ToString());
        }
        public void processActionCookies()
        {
            ListControl<T> control = new ListControl<T>();
            string action = this.GetType().Name;
            // if came from a different Action, clear the control cookies
            if (Request.Cookies[actionCookieToken] != null)
                if (Request.Cookies[actionCookieToken].Value != action)
                    control.clearCookies(Request.Cookies);
            Response.Cookies[actionCookieToken].Value = action;
        }
    }
}