namespace NGrid.Core
{
    using System.Linq.Expressions;
    using MediatR;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Reflection;
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;

    public abstract class FetchDataHandler<TViewModel, TDomain> : IAsyncRequestHandler<FetchDataQuery<TViewModel, TDomain>, FetchDataResult<TDomain>>
        where TViewModel : class
    {

        protected abstract IQueryable<TViewModel> Dataset { get; }

        public async Task<FetchDataResult<TDomain>> Handle(FetchDataQuery<TViewModel, TDomain> message)
        {
            var data = Dataset;

            var columns = GetViewModelColumnsToDisplay();

            var sortColumns = message?.SortColumns ?? GetDefaultSorts().ToArray();
            
            data = SortData(sortColumns, columns, data);
            return new FetchDataResult<TDomain>
            {
                Columns = columns,
                Data = Mapper.Map<TDomain[]>(await data.ToArrayAsync()),
                SortColumns = sortColumns
            };
        }

        private static IEnumerable<SortOption> GetDefaultSorts()
        {
            var defaultSortColumns = typeof (TDomain).GetProperties()
                .Where(x => x.GetCustomAttribute<GridAttributes.DefaultSortAttribute>() != null);

            var retVal = defaultSortColumns.OrderBy(x => x.GetCustomAttribute<GridAttributes.DefaultSortAttribute>().Order)
                .Select(x => new SortOption
                {
                    Column = x.Name.ToLowerCamelCase(),
                    SortDesc = x.GetCustomAttribute<GridAttributes.DefaultSortAttribute>().SortDescending
                });

            return retVal;
        }

        private static GridColumn[] GetViewModelColumnsToDisplay()
        {
            return typeof (TDomain).GetProperties()
                .Where(x => x.GetCustomAttribute<GridAttributes.HiddenAttribute>() == null)
                .Select(x => new GridColumn {Name = x.Name.ToLowerCamelCase()})
                .ToArray();
        }

        private IQueryable<TViewModel> SortData(IEnumerable<SortOption> sortColumns, GridColumn[] columns, IQueryable<TViewModel> data)
        {
            var first = true;
            foreach (var sortColumn in sortColumns)
            {
                var column = columns.Single(x => x.Name == sortColumn.Column);

                var columnExpr = GetColumnExpression(column);
                data = DoSort(data, first, sortColumn, columnExpr);
                first = false;
            }
            return data;
        }

        private Expression<Func<TViewModel, object>> GetColumnExpression(GridColumn column)
        {
            var propertyName = column.Name.ToUpperCamelCase();
            var mappingAttribute =
                GetPropertyMappingAttribute(propertyName);

            var columnExpr = GetQueryExpressionForColumn(mappingAttribute, propertyName);
            return columnExpr;
        }

        private Expression<Func<TViewModel, object>> GetQueryExpressionForColumn(GridAttributes.PropertyMappingAttribute mappingAttribute, string propertyName)
        {
            var columnExpr = mappingAttribute != null ? mappingAttribute.GetExpression<TViewModel>() : GetPropertySelector(propertyName);
            return columnExpr;
        }
 
        private static Expression<Func<TViewModel, object>> GetPropertySelector(string propertyName)
        {
            var arg = Expression.Parameter(typeof(TViewModel), "x");
            var property = Expression.Property(arg, propertyName);
            var conv = Expression.Convert(property, typeof(object));
            var exp = Expression.Lambda<Func<TViewModel, object>>(conv, arg);
            return exp;
        }

        private static GridAttributes.PropertyMappingAttribute GetPropertyMappingAttribute(string propertyName)
        {
            return typeof (TDomain)
                .GetProperties()
                .First(x => x.Name == propertyName)
                .GetCustomAttribute<GridAttributes.PropertyMappingAttribute>();
        }

        private static IQueryable<TViewModel> DoSort(IQueryable<TViewModel> data, bool first, SortOption sortColumn, Expression<Func<TViewModel, object>> columnExpr)
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
