namespace AccountManagement.Domain.RolePermissions.Services;

using AccountManagement.Domain.RolePermissions;
using AccountManagement.Databases;
using AccountManagement.Services;

public interface IRolePermissionRepository : IGenericRepository<RolePermission>
{
}

public sealed class RolePermissionRepository : GenericRepository<RolePermission>, IRolePermissionRepository
{
    private readonly AccountManagementDbContext _dbContext;

    public RolePermissionRepository(AccountManagementDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
}
