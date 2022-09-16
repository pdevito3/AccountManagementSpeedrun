namespace AccountManagement.Domain.RolePermissions.Mappings;

using AccountManagement.Domain.RolePermissions.Dtos;
using AccountManagement.Domain.RolePermissions;
using Mapster;

public sealed class RolePermissionMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<RolePermissionDto, RolePermission>()
            .TwoWays();
        config.NewConfig<RolePermissionForCreationDto, RolePermission>()
            .TwoWays();
        config.NewConfig<RolePermissionForUpdateDto, RolePermission>()
            .TwoWays();
    }
}