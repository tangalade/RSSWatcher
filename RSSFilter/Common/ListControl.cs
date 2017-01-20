using RSSFilter.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;

namespace RSSFilter.Common
{
    public class ListControl<T>
    {
        public const string filterCookieToken = "filters";
        public const string sorterCookieToken = "sorter";
        public const string pagerCookieToken = "pager";

        public ListControl()
        {
            parseControllableFields();
            // initialize them for view POST fields
            for (int i = 0; i < filterableFields.Count; i++)
                filters.Add(new ListFilter());
            // if no default sorter defined, pick one at random
            if (sorter == null && sortableFields.Count > 0)
                sorter = Enumerable.ToList(sortableFields.Values)[0];
        }

        public ListControl(HttpCookieCollection cookies)
        {
            parseControllableFields();
            // if no default sorter defined, pick one at random
            if (sorter == null && sortableFields.Count > 0)
                sorter = Enumerable.ToList(sortableFields.Values)[0];
            setFromCookies(cookies);
        }

        // set cookie values from ListControl values
        public void fillCookies(HttpCookieCollection cookies)
        {
            if (cookies[filterCookieToken] == null)
                cookies.Add(new HttpCookie(filterCookieToken));
            if (cookies[sorterCookieToken] == null)
                cookies.Add(new HttpCookie(sorterCookieToken));
            if (cookies[pagerCookieToken] == null)
                cookies.Add(new HttpCookie(pagerCookieToken));

            foreach (var filter in filters)
            {
                cookies[filterCookieToken].Values[filter.Name] = filter.Value;
            }
            cookies[sorterCookieToken].Values["Name"] = sorter.Name;
            cookies[sorterCookieToken].Values["Ascending"] = sorter.Ascending.ToString();
            cookies[pagerCookieToken].Values["PageSize"] = pager.PageSize.ToString();
            cookies[pagerCookieToken].Values["PageNum"] = pager.PageNum.ToString();
        }
        // set ListControl values from cookies
        public void setFromCookies(HttpCookieCollection cookies)
        {
            if (cookies[filterCookieToken] != null && cookies[filterCookieToken].HasKeys)
            {
                foreach (var filterCookie in cookies[filterCookieToken].Values)
                {
                    if (filterableFields.ContainsKey((string)filterCookie))
                    {
                        var filter = filterableFields[(string)filterCookie];
                        filter.Value = cookies[filterCookieToken].Values[(string)filterCookie];
                        filters.Add(filter);
                    }
                }
            }
            if (cookies[sorterCookieToken] != null && cookies[sorterCookieToken].HasKeys)
            {
                if (cookies[sorterCookieToken].Values.AllKeys.Contains("Name")
                    && cookies[sorterCookieToken].Values.AllKeys.Contains("Ascending"))
                {
                    if (sortableFields.ContainsKey(cookies[sorterCookieToken].Values["Name"]))
                    {
                        sorter = sortableFields[cookies[sorterCookieToken].Values["Name"]];
                        sorter.Ascending = Boolean.Parse(cookies[sorterCookieToken].Values["Ascending"]);
                    }
                }
            }
            if (cookies[pagerCookieToken] != null && cookies[pagerCookieToken].HasKeys)
            {
                if (cookies[pagerCookieToken].Values.AllKeys.Contains("PageSize"))
                    pager.PageSize = Int32.Parse(cookies[pagerCookieToken].Values["PageSize"]);
                if (cookies[pagerCookieToken].Values.AllKeys.Contains("PageNum"))
                    pager.PageNum = Int32.Parse(cookies[pagerCookieToken].Values["PageNum"]);
            }
        }
        // uses Filterable and Sortable attributes of T to fill valid fields
        private void parseControllableFields()
        {
            var props = typeof(T).GetProperties();
            foreach (var prop in props)
            {
                var filterAttr = (Filterable)prop.GetCustomAttribute(typeof(Filterable));
                if (filterAttr != null)
                {
                    var propType = prop.PropertyType;
                    if (filterAttr.propTree != null)
                    {
                        // FIXME: no checking on validity of prop tree
                        var tokens = filterAttr.propTree.Split(new char[] { '.' });
                        for (int i = 0; i < tokens.Length; i++)
                        {
                            var subProp = propType.GetProperty(tokens[i]);
                            propType = subProp.PropertyType;
                        }
                    }
                    filterableFields.Add(prop.Name, new Common.ListFilter()
                    {
                        Name = prop.Name,
                        propTree = filterAttr.propTree,
                        propType = propType,
                        filterMethod = filterAttr.filterMethod
                    });
                }
                var sortAttr = (Sortable)prop.GetCustomAttribute(typeof(Sortable));
                if (sortAttr != null)
                {
                    var propType = prop.PropertyType;
                    if (sortAttr.propTree != null)
                    {
                        // FIXME: no checking on validity of prop tree
                        var tokens = sortAttr.propTree.Split(new char[] { '.' });
                        for (int i = 0; i < tokens.Length; i++)
                        {
                            var subProp = propType.GetProperty(tokens[i]);
                            propType = subProp.PropertyType;
                        }
                    }
                    sortableFields.Add(prop.Name, new SortingToken()
                    {
                        Ascending = sortAttr.ascending,
                        Name = prop.Name,
                        propTree = sortAttr.propTree,
                        propType = propType
                    });
                    if (sortAttr.isDefault)
                        sorter = sortableFields[prop.Name];
                }
            }
        }

        // returns the predicate for a sorter based upon <prop>
        // if no <field> specified, pick one at random
        public Expression<Func<T,TKey>> getSortPredicate<TKey>(string prop = null)
        {
            // if no field specified, pick the first sortable field
            if (prop == null && sortableFields.Count > 0)
                prop = Enumerable.ToList(sortableFields.Values)[0].Name;
            // if the prop is not a sortable prop, return null
            if (sortableFields[prop] == null)
                return null;
            // create parameter of the expression of type <itemType> named "t"
            ParameterExpression param = Expression.Parameter(typeof(T), "t");
            // create property attached to the parameter
            MemberExpression memberExpression = Expression.Property(param, typeof(T).GetProperty(prop));
            var propType = typeof(T).GetProperty(prop).PropertyType;
            if (sortableFields[prop].propTree != null)
            {
                var tokens = sortableFields[prop].propTree.Split(new char[] { '.' });
                for (int i = 0; i < tokens.Length; i++)
                {
                    var subProp = propType.GetProperty(tokens[i]);
                    propType = subProp.PropertyType;
                    memberExpression = Expression.Property(memberExpression, subProp);
                }
            }
            //for (int i=0; i<sortableFields[prop].propTree.Length; i++)
            //{
            //    var subProp = propType.GetProperty(sortableFields[prop].propTree[i]);
            //    propType = subProp.PropertyType;
            //    memberExpression = Expression.Property(memberExpression, subProp);
            //}
            // create lambda expression
            Expression<Func<T, TKey>> lambda = Expression.Lambda<Func<T, TKey>>(memberExpression, param);
            return lambda;
        }

        // returns the predicate for a field based upon <prop> and <value>
        public Expression<Func<T,bool>> getFilterPredicate(string prop, string value)
        {
            // FIXME: not done correctly, not sure how to dynamically build
            // if the prop is not a filterable prop, return null
            if (filterableFields[prop] == null)
                return null;

            // create parameter of the expression of type <itemType> named "t"
            ParameterExpression param = Expression.Parameter(typeof(T), "t");
            // create property attached to the parameter
            MemberExpression memberExpression = Expression.Property(param, typeof(T).GetProperty(prop));
            var propType = typeof(T).GetProperty(prop).PropertyType;
            if (sortableFields[prop].propTree != null)
            {
                var tokens = sortableFields[prop].propTree.Split(new char[] { '.' });
                for (int i = 0; i < tokens.Length; i++)
                {
                    var subProp = propType.GetProperty(tokens[i]);
                    propType = subProp.PropertyType;
                    memberExpression = Expression.Property(memberExpression, subProp);
                }
            }
            BinaryExpression boolExpression = filterableFields[prop].filterMethod.getBinaryExpression(memberExpression, value);

            Expression<Func<T, bool>> lambda = Expression.Lambda<Func<T, bool>>(boolExpression, param);
            return lambda;
        }
        public Dictionary<string, ListFilter> filterableFields { get; set; } = new Dictionary<string, ListFilter>();
        public Dictionary<string, SortingToken> sortableFields { get; set; } = new Dictionary<string, SortingToken>();
        // set by View
        public List<ListFilter> filters { get; set; } = new List<ListFilter>();
        public SortingToken sorter { get; set; }
        public Paging pager { get; set; } = new Paging();
    }
}