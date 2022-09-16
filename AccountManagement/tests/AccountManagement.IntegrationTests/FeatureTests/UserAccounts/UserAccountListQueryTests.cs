namespace AccountManagement.IntegrationTests.FeatureTests.UserAccounts;

using AccountManagement.Domain.UserAccounts.Dtos;
using AccountManagement.SharedTestHelpers.Fakes.UserAccount;
using SharedKernel.Exceptions;
using AccountManagement.Domain.UserAccounts.Features;
using FluentAssertions;
using NUnit.Framework;
using System.Threading.Tasks;
using static TestFixture;
using AccountManagement.SharedTestHelpers.Fakes.User;

public class UserAccountListQueryTests : TestBase
{
    
    [Test]
    public async Task can_get_useraccount_list()
    {
        // Arrange
        var fakeUserAccountOne = FakeUserAccount.Generate(new FakeUserAccountForCreationDto().Generate());
        var fakeUserAccountTwo = FakeUserAccount.Generate(new FakeUserAccountForCreationDto().Generate());
        var queryParameters = new UserAccountParametersDto();

        await InsertAsync(fakeUserAccountOne, fakeUserAccountTwo);

        // Act
        var query = new GetUserAccountList.Query(queryParameters);
        var userAccounts = await SendAsync(query);

        // Assert
        userAccounts.Count.Should().BeGreaterThanOrEqualTo(2);
    }
}