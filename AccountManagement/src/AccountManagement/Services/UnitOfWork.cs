namespace AccountManagement.Services;

using AccountManagement.Databases;

public interface IUnitOfWork : IAccountManagementService
{
    Task<int> CommitChanges(CancellationToken cancellationToken = default);
}

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly AccountManagementDbContext _dbContext;

    public UnitOfWork(AccountManagementDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<int> CommitChanges(CancellationToken cancellationToken = default)
    {
        return await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
