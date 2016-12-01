namespace NGridSample.Features.Home
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


        public class FetchDataQuery<T> : IAsyncRequest<FetchDataResult<T>> where T : class
        {
            public SortOption[] SortColumns { get; set; }
        }
        
        public class FetchDataResult<T>
        {
            public Column[] Columns { get; set; }
            public T[] Data { get; set; }
        }

        public class FetchDataHandlerSampleItem : FetchDataHandler<SampleItem>
        {
            public FetchDataHandlerSampleItem(ApiContext context) : base(context)
            {
            }
        }



        public class FetchDataHandler<T> : IAsyncRequestHandler<FetchDataQuery<T>, FetchDataResult<T>> where T : class
        {
            private readonly ApiContext _context;

            public FetchDataHandler(ApiContext context)
            {
                _context = context;
            }

            private Expression<Func<T, object>> GetPropertySelector(string propertyName)
            {
                var arg = Expression.Parameter(typeof (T), "x");
                var property = Expression.Property(arg, propertyName);
                var conv = Expression.Convert(property, typeof (object));
                var exp = Expression.Lambda<Func<T, object>>(conv, arg);
                return exp;
            }

            public async Task<FetchDataResult<T>> Handle(FetchDataQuery<T> message)
            {
                IQueryable<T> data = _context.Set<T>();
                var columns = new [] {new Column { Name = "Column1", Sorted=false, SortedDesc=false}, new Column { Name = "Column2", Sorted = false, SortedDesc = false } };

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
}
