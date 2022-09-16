namespace AccountManagement.IntegrationTests.FeatureTests.RolePermissions;

using AccountManagement.SharedTestHelpers.Fakes.RolePermission;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Threading.Tasks;
using AccountManagement.Domain.RolePermissions.Features;
using static TestFixture;
using SharedKernel.Exceptions;

public class AddRolePermissionCommandTests : TestBase
{
    [Test]
    public async Task can_add_new_rolepermission_to_db()
    {
        // Arrange
        var fakeRolePermissionOne = new FakeRolePermissionForCreationDto().Generate();

        // Act
        var command = new AddRolePermission.Command(fakeRolePermissionOne);
        var rolePermissionReturned = await SendAsync(command);
        var rolePermissionCreated = await ExecuteDbContextAsync(db => db.RolePermissions
            .FirstOrDefaultAsync(r => r.Id == rolePermissionReturned.Id));

        // Assert
        rolePermissionReturned.Should().BeEquivalentTo(fakeRolePermissionOne, options =>
            options.ExcludingMissingMembers()
                .Excluding(x => x.Role));
        rolePermissionReturned.Role.Should().Be(fakeRolePermissionOne.Role);
        rolePermissionCreated.Should().BeEquivalentTo(fakeRolePermissionOne, options =>
            options.ExcludingMissingMembers()
                .Excluding(x => x.Role));
        rolePermissionCreated.Role.Value.Should().Be(fakeRolePermissionOne.Role);
    }
}