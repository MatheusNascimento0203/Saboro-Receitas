namespace Saboro.Core.Interfaces.Repositories.Base;

public interface IBaseRepository
{
    Task<IDbTransaction> BeginTransactionAsync();
    IDbTransaction BeginTransaction();
}