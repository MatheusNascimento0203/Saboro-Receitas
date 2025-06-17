using Saboro.Core.Interfaces.Repositories.Base;
using Saboro.Data.Context;

namespace Saboro.Data.Repositories.Base;

public abstract class BaseRepository(ApplicationDbContext dbContext) : IBaseRepository
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task<IDbTransaction> BeginTransactionAsync()
    {
        var transaction = await _dbContext.Database.BeginTransactionAsync();
        return new DbTransaction(transaction, _dbContext);
    }

    public IDbTransaction BeginTransaction()
    {
        var transaction = _dbContext.Database.BeginTransaction();
        return new DbTransaction(transaction, _dbContext);
    }
}
