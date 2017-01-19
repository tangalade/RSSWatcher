using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace RSSFilter.Common
{
    public class Filterable : Attribute
    {
        public Filterable() { }
        public Filterable(Type filterMethodType)
        {
            if (typeof(FilterMethod).IsAssignableFrom(filterMethodType))
                this.filterMethod = (FilterMethod)System.Activator.CreateInstance(filterMethodType);
        }
        public Filterable(string propTree)
        {
            this.propTree = propTree;
        }
        public Filterable(string propTree, Type filterMethodType)
        {
            this.propTree = propTree;
            if (filterMethodType.IsAssignableFrom(typeof(FilterMethod)))
                this.filterMethod = (FilterMethod)System.Activator.CreateInstance(filterMethodType);
        }
        // FIXME: doesn't support arbitrary prop trees, like those with array indices, map keys, or methods
        public string propTree { get; set; }
        public FilterMethod filterMethod { get; set; } = new DefaultFilterMethod();
    }

    public class ListFilter
    {
        public FilterMethod filterMethod { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        // FIXME: doesn't support arbitrary prop trees, like those with array indices, map keys, or methods
        public string propTree { get; set; }
        public Type propType { get; set; }
    }
}