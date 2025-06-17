using Saboro.Core.Interfaces.Repositories.Base;
using Saboro.Data.Context;
using Microsoft.EntityFrameworkCore.Storage;

namespace Saboro.Data.Repositories.Base;

public class DbTransaction : IDbTransaction
{
    private readonly IDbContextTransaction _dbContextTransaction;
    private readonly ApplicationDbContext _dbContext;


    public DbTransaction(IDbContextTransaction dbContextTransaction, ApplicationDbContext dbContext)
    {
        _dbContextTransaction = dbContextTransaction;
        _dbContext = dbContext;
    }

    public async Task CommitAsync()
    {
        await _dbContextTransaction.CommitAsync();
        _dbContext.ChangeTracker.Clear();
    }

    public void Commit()
    {
        _dbContextTransaction.Commit();
        _dbContext.ChangeTracker.Clear();
    }

    public async Task RollbackAsync()
    {
        await _dbContextTransaction.RollbackAsync();
        _dbContext.ChangeTracker.Clear();
    }

    public void Rollback()
    {
        _dbContextTransaction.Rollback();
        _dbContext.ChangeTracker.Clear();
    }
}