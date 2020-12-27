using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace MVCParser.Controllers
{
    internal static class DataModelExtensions
    {

        public static IQueryable<TEntity> OrderBy<TEntity>(this IQueryable<TEntity> source, string orderByProperty)
        {

            string command = "OrderBy";
            var property = source.ElementType.GetProperty(orderByProperty);
            var parameter = Expression.Parameter(source.ElementType, "p");
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            var orderByExpression = Expression.Lambda(propertyAccess, parameter);
            var resultExpression = Expression.Call(typeof(Queryable), command, new Type[] { source.ElementType, property.PropertyType },
                                              source.AsQueryable().Expression, Expression.Quote(orderByExpression));
            return source.AsQueryable().Provider.CreateQuery<TEntity>(resultExpression);



        }
        public static IQueryable<TEntity> OrderByDescending<TEntity>(this IQueryable<TEntity> source, string orderByProperty)
        {

            string command = "OrderByDescending";
            var property = source.ElementType.GetProperty(orderByProperty);
            var parameter = Expression.Parameter(source.ElementType, "p");
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            var orderByExpression = Expression.Lambda(propertyAccess, parameter);
            var resultExpression = Expression.Call(typeof(Queryable), command, new Type[] { source.ElementType, property.PropertyType },
                                          source.AsQueryable().Expression, Expression.Quote(orderByExpression));
            return source.AsQueryable().Provider.CreateQuery<TEntity>(resultExpression);


        }
    }
}