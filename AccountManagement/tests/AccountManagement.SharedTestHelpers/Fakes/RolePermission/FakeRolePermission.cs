namespace AccountManagement.SharedTestHelpers.Fakes.RolePermission;

using AutoBogus;
using AccountManagement.Domain.RolePermissions;
using AccountManagement.Domain.RolePermissions.Dtos;

public class FakeRolePermission
{
    public static RolePermission Generate(RolePermissionForCreationDto rolePermissionForCreationDto)
    {
        return RolePermission.Create(rolePermissionForCreationDto);
    }

    public static RolePermission Generate()
    {
        return RolePermission.Create(new FakeRolePermissionForCreationDto().Generate());
    }
}