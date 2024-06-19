using Entities.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Persistence.DbContexts
{
    internal sealed class SoftDeleteInterceptor(string userId) : SaveChangesInterceptor
    {        
        public override InterceptionResult<int> SavingChanges(
            DbContextEventData eventData,
            InterceptionResult<int> result)
        {
            if (eventData.Context is null)
                return result;

            foreach (var entry in eventData.Context.ChangeTracker.Entries())
            {
                if (entry is not { State: EntityState.Deleted })
                    continue;

                if (entry is { State: EntityState.Deleted, Entity: BaseEntity deleteEntity })
                {
                    entry.State = EntityState.Modified;
                    deleteEntity.IsDeleted = true;
                    deleteEntity.DeletedBy = userId;
                    deleteEntity.DeletedAt = DateTime.UtcNow;
                }                
                else
                {
                    entry.State = EntityState.Modified;
                    entry.CurrentValues["IsDeleted"] = true;
                    entry.CurrentValues["DeletedBy"] = userId;
                    entry.CurrentValues["DeletedAt"] = DateTime.UtcNow; ;
                }
            }

            return result;
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default)
        {
            if (eventData.Context is null)
                return new ValueTask<InterceptionResult<int>>(result);

            foreach (var entry in eventData.Context.ChangeTracker.Entries())
            {
                if (entry is not { State: EntityState.Deleted })
                    continue;

                if (entry is { State: EntityState.Deleted, Entity: BaseEntity deleteEntity })
                {
                    entry.State = EntityState.Modified;
                    deleteEntity.IsDeleted = true;
                    deleteEntity.DeletedBy = userId;
                    deleteEntity.DeletedAt = DateTime.UtcNow;
                }
                else
                {
                    entry.State = EntityState.Modified;
                    entry.CurrentValues["IsDeleted"] = true;
                    entry.CurrentValues["DeletedBy"] = userId;
                    entry.CurrentValues["DeletedAt"] = DateTime.UtcNow; ;
                }
            }

            return new ValueTask<InterceptionResult<int>>(result);
        }
    }
}
