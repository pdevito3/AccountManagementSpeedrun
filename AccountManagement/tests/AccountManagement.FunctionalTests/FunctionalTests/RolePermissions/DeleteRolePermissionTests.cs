namespace AccountManagement.FunctionalTests.FunctionalTests.RolePermissions;

using AccountManagement.SharedTestHelpers.Fakes.RolePermission;
using AccountManagement.FunctionalTests.TestUtilities;
using AccountManagement.Domain;
using SharedKernel.Domain;
using FluentAssertions;
using NUnit.Framework;
using System.Net;
using System.Threading.Tasks;

public class DeleteRolePermissionTests : TestBase
{
    [Test]
    public async Task delete_rolepermission_returns_nocontent_when_entity_exists_and_auth_credentials_are_valid()
    {
        // Arrange
        var fakeRolePermission = FakeRolePermission.Generate(new FakeRolePermissionForCreationDto().Generate());

        var user = await AddNewSuperAdmin();
        _client.AddAuth(user.Identifier);
        await InsertAsync(fakeRolePermission);

        // Act
        var route = ApiRoutes.RolePermissions.Delete.Replace(ApiRoutes.RolePermissions.Id, fakeRolePermission.Id.ToString());
        var result = await _client.DeleteRequestAsync(route);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
            
    [Test]
    public async Task delete_rolepermission_returns_unauthorized_without_valid_token()
    {
        // Arrange
        var fakeRolePermission = FakeRolePermission.Generate(new FakeRolePermissionForCreationDto().Generate());

        await InsertAsync(fakeRolePermission);

        // Act
        var route = ApiRoutes.RolePermissions.Delete.Replace(ApiRoutes.RolePermissions.Id, fakeRolePermission.Id.ToString());
        var result = await _client.DeleteRequestAsync(route);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
            
    [Test]
    public async Task delete_rolepermission_returns_forbidden_without_proper_scope()
    {
        // Arrange
        var fakeRolePermission = FakeRolePermission.Generate(new FakeRolePermissionForCreationDto().Generate());
        _client.AddAuth();

        await InsertAsync(fakeRolePermission);

        // Act
        var route = ApiRoutes.RolePermissions.Delete.Replace(ApiRoutes.RolePermissions.Id, fakeRolePermission.Id.ToString());
        var result = await _client.DeleteRequestAsync(route);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}