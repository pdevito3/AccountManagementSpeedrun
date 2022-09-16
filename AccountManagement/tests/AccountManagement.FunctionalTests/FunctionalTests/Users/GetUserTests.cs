namespace AccountManagement.FunctionalTests.FunctionalTests.Users;

using AccountManagement.SharedTestHelpers.Fakes.User;
using AccountManagement.FunctionalTests.TestUtilities;
using AccountManagement.Domain;
using SharedKernel.Domain;
using FluentAssertions;
using NUnit.Framework;
using System.Net;
using System.Threading.Tasks;

public class GetUserTests : TestBase
{
    [Test]
    public async Task get_user_returns_success_when_entity_exists_using_valid_auth_credentials()
    {
        // Arrange
        var fakeUser = FakeUser.Generate(new FakeUserForCreationDto().Generate());

        var user = await AddNewSuperAdmin();
        _client.AddAuth(user.Identifier);
        await InsertAsync(fakeUser);

        // Act
        var route = ApiRoutes.Users.GetRecord.Replace(ApiRoutes.Users.Id, fakeUser.Id.ToString());
        var result = await _client.GetRequestAsync(route);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }
            
    [Test]
    public async Task get_user_returns_unauthorized_without_valid_token()
    {
        // Arrange
        var fakeUser = FakeUser.Generate(new FakeUserForCreationDto().Generate());

        await InsertAsync(fakeUser);

        // Act
        var route = ApiRoutes.Users.GetRecord.Replace(ApiRoutes.Users.Id, fakeUser.Id.ToString());
        var result = await _client.GetRequestAsync(route);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
            
    [Test]
    public async Task get_user_returns_forbidden_without_proper_scope()
    {
        // Arrange
        var fakeUser = FakeUser.Generate(new FakeUserForCreationDto().Generate());
        _client.AddAuth();

        await InsertAsync(fakeUser);

        // Act
        var route = ApiRoutes.Users.GetRecord.Replace(ApiRoutes.Users.Id, fakeUser.Id.ToString());
        var result = await _client.GetRequestAsync(route);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}