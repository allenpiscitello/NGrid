namespace NGridSample.Features.Grid
{
    using System;
    using System.Threading.Tasks;
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

        public class FetchDataQuery : IAsyncRequest<object>
        {
            public SortOption[] ColumnsToSort { get; set; }
        }

        public class FetchDataResult : IAsyncRequestHandler<FetchDataQuery, object>
        {
            private readonly ApiContext _context;

            public FetchDataResult(ApiContext context)
            {
                _context = context;
            }

            public async Task<object> Handle(FetchDataQuery message)
            {
                var data = await _context.Items.ToListAsync();

                var columns = new object[] {new {Name = "Column1"}, new {Name = "Column2"}};
                return new
                {
                    Columns = columns,
                    Data = data
                };
            }
        }
    }
}
