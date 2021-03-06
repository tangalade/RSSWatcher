﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RSSFilter.Common
{
    public class Paging
    {
        public int[] SuggestedPageSizes { get; set; } = { 10, 25, 50, 100 };
        public int PageNum { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int NumPages { get; set; }
        public int NumRows { get; set; }

        public void reset()
        {
            PageNum = 1;
        }
    }
}