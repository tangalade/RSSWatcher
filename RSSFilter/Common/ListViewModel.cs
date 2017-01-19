using RSSFilter.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;

namespace RSSFilter.Common
{
    public class ListViewModel<T>
    {
        public List<T> items { get; set; }
        public ListControl<T> control { get; set; }

        public static async Task<ListViewModel<T>> CreateAsync(IQueryable<T> source, ListControl<T> control)
        {
            var items = source;
            foreach (var filter in control.filters)
            {
                if (filter.Name != null && filter.Value != null)
                {
                    var filterPredicate = control.getFilterPredicate(filter.Name, filter.Value);
                    items = items.Where(filterPredicate);
                }
            }
            var itemsList = items.ToList();

            // FIXME: add support for multiple sorts with ThenBy
            var sortPropType = control.sortableFields[control.sorter.Name].propType;
            var mainType = typeof(T);
            MethodInfo sortPredicateMethod = control.GetType().GetMethod("getSortPredicate").MakeGenericMethod(new Type[] { sortPropType });
            var sortPredicate = sortPredicateMethod.Invoke(control, new object[] { control.sorter.Name });

            var orderByMethod = typeof(Queryable).GetMethods().Where(x => x.Name == "OrderBy").Where(x => x.GetParameters().Length == 2).SingleOrDefault();
            MethodInfo orderByGenMethod = orderByMethod.MakeGenericMethod(new Type[] { mainType, sortPropType });
            var orderByDescMethod = typeof(Queryable).GetMethods().Where(x => x.Name == "OrderByDescending").Where(x => x.GetParameters().Length == 2).SingleOrDefault();
            MethodInfo orderByDescGenMethod = orderByDescMethod.MakeGenericMethod(new Type[] { mainType, sortPropType });
            if (control.sorter.Ascending)
                items = (IQueryable<T>)orderByGenMethod.Invoke(items, new object[] { items, sortPredicate });
            else
                items = (IQueryable<T>)orderByDescGenMethod.Invoke(items, new object[] { items, sortPredicate });
            itemsList = items.ToList();


            var count = await items.CountAsync();
            control.pager.NumPages = count / control.pager.PageSize + 1;
            var itemsPage = await items.Skip((control.pager.PageNum - 1) * control.pager.PageSize).Take(control.pager.PageSize).ToListAsync();

            return new ListViewModel<T>() { items = itemsPage, control = control };
        }
    }
}