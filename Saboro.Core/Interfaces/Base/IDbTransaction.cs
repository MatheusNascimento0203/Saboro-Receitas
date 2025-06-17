namespace Saboro.Core.Interfaces.Repositories.Base;

public interface IDbTransaction
{
    Task CommitAsync();
    void Commit();

    Task RollbackAsync();
    void Rollback();
}