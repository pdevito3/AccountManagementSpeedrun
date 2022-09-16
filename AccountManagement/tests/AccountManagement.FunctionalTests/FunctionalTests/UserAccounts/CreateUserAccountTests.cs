namespace AccountManagement.FunctionalTests.FunctionalTests.UserAccounts;

using AccountManagement.SharedTestHelpers.Fakes.UserAccount;
using AccountManagement.FunctionalTests.TestUtilities;
using AccountManagement.Domain;
using SharedKernel.Domain;
using FluentAssertions;
using NUnit.Framework;
using System.Net;
using System.Threading.Tasks;

public class CreateUserAccountTests : TestBase
{
    [Test]
    public async Task create_useraccount_returns_created_using_valid_dto_and_valid_auth_credentials()
    {
        // Arrange
        var fakeUserAccount = new FakeUserAccountForCreationDto { }.Generate();

        var user = await AddNewSuperAdmin();
        _client.AddAuth(user.Identifier);

        // Act
        var route = ApiRoutes.UserAccounts.Create;
        var result = await _client.PostJsonRequestAsync(route, fakeUserAccount);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Created);
    }
            
    [Test]
    public async Task create_useraccount_returns_unauthorized_without_valid_token()
    {
        // Arrange
        var fakeUserAccount = new FakeUserAccountForCreationDto { }.Generate();

        // Act
        var route = ApiRoutes.UserAccounts.Create;
        var result = await _client.PostJsonRequestAsync(route, fakeUserAccount);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
            
    [Test]
    public async Task create_useraccount_returns_forbidden_without_proper_scope()
    {
        // Arrange
        var fakeUserAccount = new FakeUserAccountForCreationDto { }.Generate();
        _client.AddAuth();

        // Act
        var route = ApiRoutes.UserAccounts.Create;
        var result = await _client.PostJsonRequestAsync(route, fakeUserAccount);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}