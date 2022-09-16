namespace AccountManagement.FunctionalTests.FunctionalTests.Users;

using AccountManagement.SharedTestHelpers.Fakes.User;
using AccountManagement.FunctionalTests.TestUtilities;
using AccountManagement.Domain;
using SharedKernel.Domain;
using FluentAssertions;
using NUnit.Framework;
using System.Net;
using System.Threading.Tasks;

public class DeleteUserTests : TestBase
{
    [Test]
    public async Task delete_user_returns_nocontent_when_entity_exists_and_auth_credentials_are_valid()
    {
        // Arrange
        var fakeUser = FakeUser.Generate(new FakeUserForCreationDto().Generate());

        var user = await AddNewSuperAdmin();
        _client.AddAuth(user.Identifier);
        await InsertAsync(fakeUser);

        // Act
        var route = ApiRoutes.Users.Delete.Replace(ApiRoutes.Users.Id, fakeUser.Id.ToString());
        var result = await _client.DeleteRequestAsync(route);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
            
    [Test]
    public async Task delete_user_returns_unauthorized_without_valid_token()
    {
        // Arrange
        var fakeUser = FakeUser.Generate(new FakeUserForCreationDto().Generate());

        await InsertAsync(fakeUser);

        // Act
        var route = ApiRoutes.Users.Delete.Replace(ApiRoutes.Users.Id, fakeUser.Id.ToString());
        var result = await _client.DeleteRequestAsync(route);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
            
    [Test]
    public async Task delete_user_returns_forbidden_without_proper_scope()
    {
        // Arrange
        var fakeUser = FakeUser.Generate(new FakeUserForCreationDto().Generate());
        _client.AddAuth();

        await InsertAsync(fakeUser);

        // Act
        var route = ApiRoutes.Users.Delete.Replace(ApiRoutes.Users.Id, fakeUser.Id.ToString());
        var result = await _client.DeleteRequestAsync(route);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}