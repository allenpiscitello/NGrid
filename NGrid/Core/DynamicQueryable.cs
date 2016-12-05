namespace NGrid.Core
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    public static class DynamicQueryableExtensions
    {
        public static IQueryable<T> DynamicOrderBy<T>(this IQueryable<T> source, Expression<Func<T, object>> expression)
        {
            var resultExp = Expression.Call(typeof (Queryable), "OrderBy",
                new[] {typeof(T), typeof(object)}, source.Expression, Expression.Quote(expression));
            return source.Provider.CreateQuery<T>(resultExp);
        }

        public static IQueryable<T> DynamicOrderByDesc<T, U>(this IQueryable<T> source, Expression<Func<T, U>> expression)
        {
            var resultExp = Expression.Call(typeof(Queryable), "OrderByDescending",
                  new[] { typeof(T), typeof(object) }, source.Expression, Expression.Quote(expression));
            return source.Provider.CreateQuery<T>(resultExp);
        }

        public static IQueryable<T> DynamicThenBy<T, U>(this IQueryable<T> source, Expression<Func<T, U>> expression)
        {
            var resultExp = Expression.Call(typeof(Queryable), "ThenBy",
                    new[] { typeof(T), typeof(object) }, source.Expression, Expression.Quote(expression));
            return source.Provider.CreateQuery<T>(resultExp);
        }

        public static IQueryable<T> DynamicThenByDesc<T, U>(this IQueryable<T> source, Expression<Func<T, U>> expression)
        {
            var resultExp = Expression.Call(typeof(Queryable), "ThenByDescending",
                new[] { typeof(T), typeof(object) }, source.Expression, Expression.Quote(expression));
            return source.Provider.CreateQuery<T>(resultExp);
        }
    }
}