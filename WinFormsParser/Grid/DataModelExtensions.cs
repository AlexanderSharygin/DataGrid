using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Parser
{
    public static class DataModelExtensions
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
     
        public static TEntity GetObjectWithEqualProperties<TEntity>(this IQueryable<TEntity> source, TEntity objectToCompare)
        {
            List<TEntity> listSource = source.ToList();
              
       
            var resultObject = Activator.CreateInstance(listSource.First().GetType());
            foreach (var item in listSource)
            {
                bool isFinded = true;
                PropertyDescriptorCollection propertiesOfObject = TypeDescriptor.GetProperties(objectToCompare);
                PropertyDescriptorCollection propertiesOfCollectionItem = TypeDescriptor.GetProperties(item);
                foreach (PropertyDescriptor prop in propertiesOfObject)
                {
                    var tempProp = propertiesOfCollectionItem.Find(prop.Name, false);
                    var tempPropValue = tempProp.GetValue(item);
                    var objectPropValue = prop.GetValue(objectToCompare);
                    if (tempProp.GetValue(item).ToString() != prop.GetValue(objectToCompare).ToString())
                    {
                        isFinded = false;
                        break;
                    }
                }
                if (!isFinded)
                {
                    continue;
                }
                else
                {
                    resultObject = item;
                    break;
                }
            }
            return (TEntity)resultObject;
        }

    }

}
