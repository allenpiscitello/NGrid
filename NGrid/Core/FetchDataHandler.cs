namespace NGrid.Core
{
    using System.Linq.Expressions;
    using MediatR;
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Reflection;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Microsoft.EntityFrameworkCore;

    public abstract class FetchDataHandler<T, U> : IAsyncRequestHandler<FetchDataQuery<T, U>, FetchDataResult<U>>
        where T : class
    {

        protected abstract IQueryable<T> Dataset { get; }

        protected virtual Expression<Func<T, object>> GetPropertySelector(string propertyName)
        {
            var arg = Expression.Parameter(typeof (T), "x");
            var property = Expression.Property(arg, propertyName);
            var conv = Expression.Convert(property, typeof (object));
            var exp = Expression.Lambda<Func<T, object>>(conv, arg);
            return exp;
        }

        public async Task<FetchDataResult<U>> Handle(FetchDataQuery<T, U> message)
        {
            IQueryable<T> data = Dataset;

            var columns =
                typeof (U).GetProperties()
                    .Where(x => x.GetCustomAttribute<GridAttributes.HiddenAttribute>() == null)
                    .Select(x => new GridColumn {Name = x.Name.ToLowerCamelCase()})
                    .ToArray();

            if (message?.SortColumns != null)
            {
                var first = true;
                foreach (var sortColumn in message.SortColumns)
                {
                    var column = columns.Single(x => x.Name == sortColumn.Column);
                    column.Sorted = true;
                    var columnExpr = GetPropertySelector(column.Name.ToUpperCamelCase());
                    column.SortedDesc = sortColumn.SortDesc;
                    if (first)
                    {
                        data = sortColumn.SortDesc
                            ? data.DynamicOrderByDesc(columnExpr)
                            : data.DynamicOrderBy(columnExpr);

                        first = false;
                    }
                    else
                    {
                        data = sortColumn.SortDesc ? data.DynamicThenByDesc(columnExpr) : data.DynamicThenBy(columnExpr);
                    }
                }
            }
            
            return new FetchDataResult<U>
            {
                Columns = columns,
                Data = Mapper.Map<U[]>(await data.ToArrayAsync())
            };
        }

    }

    public static class StringConversions
    {
        public static string ToLowerCamelCase(this string val)
        {
            var returnVal = char.ToLowerInvariant(val.First()) + val.Substring(1);
            return returnVal;
        }

        public static string ToUpperCamelCase(this string val)
        {
            var returnVal = char.ToUpperInvariant(val.First()) + val.Substring(1);
            return returnVal;
        }
    }
}
