namespace AccountManagement.IntegrationTests.FeatureTests.RolePermissions;

using AccountManagement.SharedTestHelpers.Fakes.RolePermission;
using AccountManagement.Domain.RolePermissions.Dtos;
using SharedKernel.Exceptions;
using AccountManagement.Domain.RolePermissions.Features;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Threading.Tasks;
using static TestFixture;

public class UpdateRolePermissionCommandTests : TestBase
{
    [Test]
    public async Task can_update_existing_rolepermission_in_db()
    {
        // Arrange
        var fakeRolePermissionOne = FakeRolePermission.Generate(new FakeRolePermissionForCreationDto().Generate());
        var updatedRolePermissionDto = new FakeRolePermissionForUpdateDto().Generate();
        await InsertAsync(fakeRolePermissionOne);

        var rolePermission = await ExecuteDbContextAsync(db => db.RolePermissions
            .FirstOrDefaultAsync(r => r.Id == fakeRolePermissionOne.Id));
        var id = rolePermission.Id;

        // Act
        var command = new UpdateRolePermission.Command(id, updatedRolePermissionDto);
        await SendAsync(command);
        var updatedRolePermission = await ExecuteDbContextAsync(db => db.RolePermissions.FirstOrDefaultAsync(r => r.Id == id));

        // Assert
        updatedRolePermission.Should().BeEquivalentTo(updatedRolePermissionDto, options =>
            options.ExcludingMissingMembers()
                .Excluding(x => x.Role));
        updatedRolePermission.Role.Value.Should().Be(updatedRolePermissionDto.Role);
    }
}