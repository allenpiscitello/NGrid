namespace NGrid.Core
{
    using System.Linq.Expressions;
    using MediatR;
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Reflection;
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;

    public abstract class FetchDataHandler<T, U> : IAsyncRequestHandler<FetchDataQuery<T, U>, FetchDataResult<U>>
        where T : class
    {

        protected abstract IQueryable<T> Dataset { get; }

        private Expression<Func<T, object>> GetPropertySelector(string propertyName)
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
                data = SortData(message, columns, data);
            }

            return new FetchDataResult<U>
            {
                Columns = columns,
                Data = Mapper.Map<U[]>(await data.ToArrayAsync()),
                SortColumns = message?.SortColumns ?? new SortOption[0]
            };
        }

        private IQueryable<T> SortData(FetchDataQuery<T, U> message, GridColumn[] columns, IQueryable<T> data)
        {
            var first = true;
            foreach (var sortColumn in message.SortColumns)
            {
                var column = columns.Single(x => x.Name == sortColumn.Column);
                column.Sorted = true;

                var columnExpr = GetColumnExpression(column, sortColumn);
                data = DoSort(data, first, sortColumn, columnExpr);
                first = false;
            }
            return data;
        }

        private Expression<Func<T, object>> GetColumnExpression(GridColumn column, SortOption sortColumn)
        {
            var propertyName = column.Name.ToUpperCamelCase();
            var mappingAttribute =
                GetPropertyMappingAttribute(propertyName);

            var columnExpr = GetQueryExpressionForColumn(mappingAttribute, propertyName);
            column.SortedDesc = sortColumn.SortDesc;
            return columnExpr;
        }

        private Expression<Func<T, object>> GetQueryExpressionForColumn(GridAttributes.PropertyMappingAttribute mappingAttribute, string propertyName)
        {
            var columnExpr = mappingAttribute != null ? mappingAttribute.GetExpression<T>() : GetPropertySelector(propertyName);
            return columnExpr;
        }

        private static GridAttributes.PropertyMappingAttribute GetPropertyMappingAttribute(string propertyName)
        {
            return typeof (U)
                .GetProperties()
                .First(x => x.Name == propertyName)
                .GetCustomAttribute<GridAttributes.PropertyMappingAttribute>();
        }

        private static IQueryable<T> DoSort(IQueryable<T> data, bool first, SortOption sortColumn, Expression<Func<T, object>> columnExpr)
        {
            if (first)
            {
                data = sortColumn.SortDesc
                    ? data.DynamicOrderByDesc(columnExpr)
                    : data.DynamicOrderBy(columnExpr);
            }
            else
            {
                data = sortColumn.SortDesc ? data.DynamicThenByDesc(columnExpr) : data.DynamicThenBy(columnExpr);
            }
            return data;
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
