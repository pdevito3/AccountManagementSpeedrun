namespace AccountManagement.FunctionalTests.FunctionalTests.UserAccounts;

using AccountManagement.SharedTestHelpers.Fakes.UserAccount;
using AccountManagement.FunctionalTests.TestUtilities;
using AccountManagement.Domain;
using SharedKernel.Domain;
using FluentAssertions;
using NUnit.Framework;
using System.Net;
using System.Threading.Tasks;

public class DeleteUserAccountTests : TestBase
{
    [Test]
    public async Task delete_useraccount_returns_nocontent_when_entity_exists_and_auth_credentials_are_valid()
    {
        // Arrange
        var fakeUserAccount = FakeUserAccount.Generate(new FakeUserAccountForCreationDto().Generate());

        var user = await AddNewSuperAdmin();
        _client.AddAuth(user.Identifier);
        await InsertAsync(fakeUserAccount);

        // Act
        var route = ApiRoutes.UserAccounts.Delete.Replace(ApiRoutes.UserAccounts.Id, fakeUserAccount.Id.ToString());
        var result = await _client.DeleteRequestAsync(route);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
            
    [Test]
    public async Task delete_useraccount_returns_unauthorized_without_valid_token()
    {
        // Arrange
        var fakeUserAccount = FakeUserAccount.Generate(new FakeUserAccountForCreationDto().Generate());

        await InsertAsync(fakeUserAccount);

        // Act
        var route = ApiRoutes.UserAccounts.Delete.Replace(ApiRoutes.UserAccounts.Id, fakeUserAccount.Id.ToString());
        var result = await _client.DeleteRequestAsync(route);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
            
    [Test]
    public async Task delete_useraccount_returns_forbidden_without_proper_scope()
    {
        // Arrange
        var fakeUserAccount = FakeUserAccount.Generate(new FakeUserAccountForCreationDto().Generate());
        _client.AddAuth();

        await InsertAsync(fakeUserAccount);

        // Act
        var route = ApiRoutes.UserAccounts.Delete.Replace(ApiRoutes.UserAccounts.Id, fakeUserAccount.Id.ToString());
        var result = await _client.DeleteRequestAsync(route);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}