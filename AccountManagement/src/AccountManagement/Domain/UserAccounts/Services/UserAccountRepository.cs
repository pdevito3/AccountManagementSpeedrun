namespace AccountManagement.Domain.UserAccounts.Services;

using AccountManagement.Domain.UserAccounts;
using AccountManagement.Databases;
using AccountManagement.Services;

public interface IUserAccountRepository : IGenericRepository<UserAccount>
{
}

public sealed class UserAccountRepository : GenericRepository<UserAccount>, IUserAccountRepository
{
    private readonly AccountManagementDbContext _dbContext;

    public UserAccountRepository(AccountManagementDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
}
