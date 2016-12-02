namespace NGrid.Core
{
    using System.Linq.Expressions;
    using MediatR;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Reflection;
    using Microsoft.EntityFrameworkCore;
    using NGridSample.Infrastructure;

    public abstract class FetchDataHandler<T> : IAsyncRequestHandler<FetchDataQuery<T>, FetchDataResult<T>> where T : class
    {

        protected abstract IQueryable<T> Dataset { get; }


        private Expression<Func<T, object>> GetPropertySelector(string propertyName)
        {
            var arg = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(arg, propertyName);
            var conv = Expression.Convert(property, typeof(object));
            var exp = Expression.Lambda<Func<T, object>>(conv, arg);
            return exp;
        }

        public async Task<FetchDataResult<T>> Handle(FetchDataQuery<T> message)
        {
            IQueryable<T> data = Dataset;

            var columns = typeof(T).GetProperties().Where(x => x.GetCustomAttribute<GridAttributes.HiddenAttribute>() == null).Select(x => new GridColumn { Name = x.Name }).ToArray();

            if (message?.SortColumns != null)
            {
                foreach (var sortColumn in message.SortColumns)
                {
                    var column = columns.Single(x => x.Name == sortColumn.Column);
                    column.Sorted = true;

                    column.SortedDesc = sortColumn.SortDesc;
                    data = sortColumn.SortDesc
                        ? data.OrderByDescending(GetPropertySelector(column.Name))
                        : data.OrderBy(GetPropertySelector(column.Name));
                }
            }

            return new FetchDataResult<T>
            {
                Columns = columns,
                Data = await data.ToArrayAsync()
            };
        }
    }

}
