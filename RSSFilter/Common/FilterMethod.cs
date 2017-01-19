using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace RSSFilter.Common
{
    public interface FilterMethod
    {
        BinaryExpression getBinaryExpression(MemberExpression memberExpression, object value);
    }
    public class DateFilterMethod : FilterMethod
    {
        public BinaryExpression getBinaryExpression(MemberExpression memberExpression, object value)
        {
            DateTimeOffset dateFilterVal = DateTimeOffset.Parse((string)value);
            MemberExpression yearCompExpr = Expression.Property(memberExpression, "Year");
            MemberExpression monthCompExpr = Expression.Property(memberExpression, "Month");
            MemberExpression dayCompExpr = Expression.Property(memberExpression, "Day");
            BinaryExpression yearExpression = Expression.Equal(yearCompExpr, Expression.Constant(dateFilterVal.Year));
            BinaryExpression monthExpression = Expression.Equal(monthCompExpr, Expression.Constant(dateFilterVal.Month));
            BinaryExpression dayExpression = Expression.Equal(dayCompExpr, Expression.Constant(dateFilterVal.Day));
            return Expression.AndAlso(yearExpression, Expression.AndAlso(monthExpression, dayExpression));
        }
    }
    public class DefaultFilterMethod : FilterMethod
    {
        public BinaryExpression getBinaryExpression(MemberExpression memberExpression, object value)
        {
            return Expression.Equal(memberExpression, Expression.Constant(value));
        }
    }
}