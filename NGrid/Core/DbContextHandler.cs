namespace NGrid.Core
{
    using Microsoft.EntityFrameworkCore;
    using System.Linq;


    public abstract class DbContextHandler<T> : FetchDataHandler<T> where T : class
    {
        private readonly DbContext _context;

        protected DbContextHandler(DbContext context)
        {
            _context = context;
        }

        protected override IQueryable<T> Dataset => _context.Set<T>();
    }

}
