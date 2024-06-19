using Entities.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.DbContexts
{
    public sealed class TenantContext : DbContext, IDisposable
    {
        public DbSet<Tenant>? Tenants { get; set; }

        public TenantContext(DbContextOptions<TenantContext> options) 
            : base(options)
        {
            //
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DatabaseContext).Assembly);
        }

        public override int SaveChanges()
        {
            return base.SaveChanges();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(
            bool acceptAllChangesOnSuccess, 
            CancellationToken cancellationToken = default)
        {
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public override ValueTask DisposeAsync()
        {
            base.DisposeAsync();
            return new ValueTask();
        }
    }
}
