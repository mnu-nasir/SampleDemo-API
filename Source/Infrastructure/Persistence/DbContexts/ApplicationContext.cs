using Contracts;
using Entities.Entities;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Persistence.DbContexts
{
    public sealed class ApplicationContext : DbContext, IDisposable
    {
        private readonly Guid _tenant;
        private readonly Guid _userAccount;

        public ApplicationContext(
            DbContextOptions<ApplicationContext> options,
            ITenantResolver tenantResolver,
            IUserAccountResolver accountResolver) : base(options)
        {
            _tenant = tenantResolver.GetCurrentTenant();
            _userAccount = accountResolver.GetCurrentUserAccount();
        }

        public DbSet<Employee>? Employees { get; set; }
        public DbSet<ApplicationUser>? ApplicationUsers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.AddInterceptors(new SoftDeleteInterceptor(_userAccount.ToString()));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationContext).Assembly);
            
            modelBuilder.Entity<Employee>().HasQueryFilter(e => e.TenantId == _tenant && !e.IsDeleted);
            modelBuilder.Entity<ApplicationUser>().HasQueryFilter(a => a.TenantId == _tenant && !a.IsDeleted);
        }

        public override int SaveChanges()
        {
            OnValidateEntitiesBeforeSaveChanges();
            OnModifyEntitiesBeforeSaveChanges();
            return base.SaveChanges();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            OnValidateEntitiesBeforeSaveChanges();
            OnModifyEntitiesBeforeSaveChanges();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(
            bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default)
        {
            OnValidateEntitiesBeforeSaveChanges();
            OnModifyEntitiesBeforeSaveChanges();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void OnValidateEntitiesBeforeSaveChanges()
        {
            ChangeTracker.DetectChanges();

            var entities = from e in ChangeTracker.Entries()
                           where e.State == EntityState.Added
                               || e.State == EntityState.Modified
                           select e.Entity;

            foreach (var entity in entities)
            {
                var validationContext = new ValidationContext(entity);
                Validator.ValidateObject(
                    entity,
                    validationContext,
                    validateAllProperties: true);
            }
        }

        private void OnModifyEntitiesBeforeSaveChanges()
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.State == EntityState.Added)
                {
                    if (entry is { Entity: BaseEntity baseEntity })
                    {
                        baseEntity.CreatedBy = _userAccount.ToString();
                        baseEntity.CreatedAt = DateTime.UtcNow;
                    }
                    else
                    {
                        entry.CurrentValues["CreatedBy"] = _userAccount.ToString();
                        entry.CurrentValues["CreatedAt"] = DateTime.UtcNow;
                    }
                }

                if (entry.State == EntityState.Modified)
                {
                    if (entry is { Entity: BaseEntity baseEntity })
                    {
                        baseEntity.ModifiedBy = _userAccount.ToString();
                        baseEntity.ModifiedAt = DateTime.UtcNow;
                    }
                    else
                    {
                        entry.CurrentValues["ModifiedBy"] = _userAccount.ToString();
                        entry.CurrentValues["ModifiedAt"] = DateTime.UtcNow;
                    }
                }
            }
        }

        public override ValueTask DisposeAsync()
        {
            base.DisposeAsync();
            return new ValueTask();
        }
    }
}
