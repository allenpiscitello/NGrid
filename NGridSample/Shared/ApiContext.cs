namespace NGridSample.Shared
{
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class ApiContext : DbContext
    {
        public ApiContext(DbContextOptions<ApiContext> options)
            : base(options)
        {
        }

        public DbSet<SampleItem> Items { get; set; }
    }
}