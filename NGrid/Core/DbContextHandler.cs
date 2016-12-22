namespace NGrid.Core
{
    using Microsoft.EntityFrameworkCore;
    using System.Linq;

    public abstract class DbContextHandler<T, U> : FetchDataHandler<T, U> where T : class
    {
        private readonly DbContext _context;

        protected DbContextHandler(DbContext context)
        {
            _context = context;
        }

        protected override IQueryable<T> Dataset => DbSet;

        protected virtual DbSet<T> DbSet => _context.Set<T>();
    }

}
