using System.Data;
using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Saboro.Data.Extensions;

public static class DbContextExtension
{
    public static async Task UpdateEntryAsync<TEntity>(this DbContext dbContext, object[] key, object modifiedFields, CancellationToken cancellationToken = default) where TEntity : class
    {
        var entity = await dbContext.Set<TEntity>().FindAsync(key, cancellationToken);
        dbContext.Attach(entity);
        dbContext.Entry(entity).CurrentValues.SetValues(modifiedFields);
    }

    public static async Task UpdateEntryAsync<TEntity>(this DbContext dbContext, object key, object modifiedFields, CancellationToken cancellationToken = default) where TEntity : class
    {
        await dbContext.UpdateEntryAsync<TEntity>(new[] { key }, modifiedFields, cancellationToken);
    }

    public static void RemoveEntry<TEntity>(this DbContext dbContext, TEntity entity) where TEntity : class
    {
        dbContext.Entry(entity).State = EntityState.Deleted;
    }
}