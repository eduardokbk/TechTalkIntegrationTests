using Microsoft.EntityFrameworkCore;

namespace TechTalkIntegrationTests.Infrastructure.Context
{
    public class MainContext : DbContext
    {
        public MainContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MainContext).Assembly);
        }
    }
}
