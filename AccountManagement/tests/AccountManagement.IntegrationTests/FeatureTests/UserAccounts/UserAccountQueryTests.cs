namespace AccountManagement.IntegrationTests.FeatureTests.UserAccounts;

using AccountManagement.SharedTestHelpers.Fakes.UserAccount;
using AccountManagement.Domain.UserAccounts.Features;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SharedKernel.Exceptions;
using System.Threading.Tasks;
using static TestFixture;
using AccountManagement.SharedTestHelpers.Fakes.User;

public class UserAccountQueryTests : TestBase
{
    [Test]
    public async Task can_get_existing_useraccount_with_accurate_props()
    {
        // Arrange
        var fakeUserAccountOne = FakeUserAccount.Generate(new FakeUserAccountForCreationDto().Generate());
        await InsertAsync(fakeUserAccountOne);

        // Act
        var query = new GetUserAccount.Query(fakeUserAccountOne.Id);
        var userAccount = await SendAsync(query);

        // Assert
        userAccount.Should().BeEquivalentTo(fakeUserAccountOne, options =>
            options.ExcludingMissingMembers());
    }

    [Test]
    public async Task get_useraccount_throws_notfound_exception_when_record_does_not_exist()
    {
        // Arrange
        var badId = Guid.NewGuid();

        // Act
        var query = new GetUserAccount.Query(badId);
        Func<Task> act = () => SendAsync(query);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }
}