using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace RSSFilter.Common
{
    public class Sortable : Attribute
    {
        public Sortable(bool isDefault = false, bool ascending = true)
        {
            this.isDefault = isDefault;
            this.ascending = ascending;
        }
        public Sortable(string propTree, bool isDefault = false, bool ascending = true)
        {
            this.propTree = propTree;
            this.isDefault = isDefault;
            this.ascending = ascending;
        }
        public bool isDefault { get; set; }
        public bool ascending { get; set; }
        // of the format "<prop1>.<prop2>...."
        public string propTree { get; set; }
    }
    public class SortingToken
    {
        public bool Ascending { get; set; }
        public string Name { get; set; }
        public string propTree { get; set; }
        public Type propType { get; set; }
    }
}