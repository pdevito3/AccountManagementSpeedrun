namespace AccountManagement.FunctionalTests.FunctionalTests.RolePermissions;

using AccountManagement.SharedTestHelpers.Fakes.RolePermission;
using AccountManagement.FunctionalTests.TestUtilities;
using AccountManagement.Domain;
using SharedKernel.Domain;
using FluentAssertions;
using NUnit.Framework;
using System.Net;
using System.Threading.Tasks;

public class CreateRolePermissionTests : TestBase
{
    [Test]
    public async Task create_rolepermission_returns_created_using_valid_dto_and_valid_auth_credentials()
    {
        // Arrange
        var fakeRolePermission = new FakeRolePermissionForCreationDto { }.Generate();

        var user = await AddNewSuperAdmin();
        _client.AddAuth(user.Identifier);

        // Act
        var route = ApiRoutes.RolePermissions.Create;
        var result = await _client.PostJsonRequestAsync(route, fakeRolePermission);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Created);
    }
            
    [Test]
    public async Task create_rolepermission_returns_unauthorized_without_valid_token()
    {
        // Arrange
        var fakeRolePermission = new FakeRolePermissionForCreationDto { }.Generate();

        // Act
        var route = ApiRoutes.RolePermissions.Create;
        var result = await _client.PostJsonRequestAsync(route, fakeRolePermission);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
            
    [Test]
    public async Task create_rolepermission_returns_forbidden_without_proper_scope()
    {
        // Arrange
        var fakeRolePermission = new FakeRolePermissionForCreationDto { }.Generate();
        _client.AddAuth();

        // Act
        var route = ApiRoutes.RolePermissions.Create;
        var result = await _client.PostJsonRequestAsync(route, fakeRolePermission);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}