using RSSFilter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RSSFilter.ViewModel
{
    public class RSSServiceFormViewModel
    {
        public IEnumerable<RSSServiceInfo> RSSServiceInfoes { get; set; }
        public RSSServiceInfo RSSServiceInfo { get; set; }
    }
}