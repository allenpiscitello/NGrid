namespace NGridSample.Infrastructure
{
    using Microsoft.EntityFrameworkCore;
    using Domain;

    public class ApiContext : DbContext
    {
        public ApiContext(DbContextOptions<ApiContext> options)
            : base(options)
        {
        }

        public DbSet<SampleItem> Items { get; set; }
        public DbSet<ChildEntity> Children { get; set; }
    }
}