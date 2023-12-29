using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistance.Configuration
{
    public class NumbersContext: DbContext
    {
        public NumbersContext(DbContextOptions<NumbersContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseSerialColumns();
        }
        public DbSet<NumberEntity> Numbers => Set<NumberEntity>();
    }
}
