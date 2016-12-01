namespace NGridSample.Features.Grid
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Domain;
    using Infrastructure;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    public class FetchData
    {
        public class SortOption
        {
            public string Column { get; set; }
            public bool SortDesc { get; set; }
        }

        public class Column
        {
            public string Name { get; set; }
            public bool Sorted { get; set; }
            public bool SortedDesc { get; set; }
        }


        public class FetchDataQuery : IAsyncRequest<object>
        {
            public SortOption[] SortColumns { get; set; }
        }

        public class FetchDataResult : IAsyncRequestHandler<FetchDataQuery, object>
        {
            private readonly ApiContext _context;

            public FetchDataResult(ApiContext context)
            {
                _context = context;
            }

            private Expression<Func<T, object>> GetPropertySelector<T>(string propertyName)
            {
                var arg = Expression.Parameter(typeof (T), "x");
                var property = Expression.Property(arg, propertyName);
                var conv = Expression.Convert(property, typeof (object));
                var exp = Expression.Lambda<Func<T, object>>(conv, arg);
                return exp;
            }

            public async Task<object> Handle(FetchDataQuery message)
            {
                IQueryable<SampleItem> data = _context.Items;
                var columns = new [] {new Column { Name = "Column1", Sorted=false, SortedDesc=false}, new Column { Name = "Column2", Sorted = false, SortedDesc = false } };

                if (message?.SortColumns != null)
                {
                    foreach (var sortColumn in message.SortColumns)
                    {
                        var column = columns.Single(x => x.Name == sortColumn.Column);
                        column.Sorted = true;


                        column.SortedDesc = sortColumn.SortDesc;
                        data = sortColumn.SortDesc
                            ? data.OrderByDescending(GetPropertySelector<SampleItem>(column.Name))
                            : data.OrderBy(GetPropertySelector<SampleItem>(column.Name));
                    }
                }

                return new
                {
                    Columns = columns,
                    Data = await data.ToListAsync()
                };
            }
        }
    }
}
