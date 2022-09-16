namespace AccountManagement.SharedTestHelpers.Fakes.RolePermission;

using AutoBogus;
using AccountManagement.Domain;
using AccountManagement.Domain.RolePermissions.Dtos;
using AccountManagement.Domain.Roles;

public class FakeRolePermissionForCreationDto : AutoFaker<RolePermissionForCreationDto>
{
    public FakeRolePermissionForCreationDto()
    {
        RuleFor(rp => rp.Permission, f => f.PickRandom(Permissions.List()));
        RuleFor(rp => rp.Role, f => f.PickRandom(Role.ListNames()));
    }
}