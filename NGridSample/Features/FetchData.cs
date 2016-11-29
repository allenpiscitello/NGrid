namespace NGridSample.Features
{
    using System.Threading.Tasks;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using Shared;

    public class FetchDataQuery : IAsyncRequest<object>
    {
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
