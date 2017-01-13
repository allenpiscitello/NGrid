namespace NGrid.Core
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using AutoMapper;

    public static class GridMappingExtensions
    {
        public static IMappingExpression<Source, Destination> ApplyGridMap<Source, Destination>(this IMappingExpression<Source, Destination> map)
        {
            var properties =
           typeof(Destination).GetProperties()
               .Where(x => x.GetCustomAttribute<GridAttributes.PropertyMappingAttribute>() != null);

            foreach (var property in properties)
            {
                map = map.ForMember(CreateExpression<Destination>(property), opt => opt.MapFrom(GetMapFromExpression<Source>(property)));
            }
            return map;
        }

        private static Expression<Func<Source, object>> GetMapFromExpression<Source>(PropertyInfo propInfo)
        {
            var attribute = propInfo.GetCustomAttribute<GridAttributes.PropertyMappingAttribute>();

            return attribute.GetExpression<Source>();
        }

        private static Expression<Func<Destination, object>> CreateExpression<Destination>(PropertyInfo propInfo)
        {
            var parameter = Expression.Parameter(typeof(Destination));
            var property = Expression.Property(parameter, propInfo);
            var conversion = Expression.Convert(property, typeof(object));
            var lambda = Expression.Lambda<Func<Destination, object>>(conversion, parameter);
            return lambda;
        }
    }
}
