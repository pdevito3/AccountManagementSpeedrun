namespace AccountManagement.IntegrationTests.FeatureTests.UserAccounts;

using AccountManagement.SharedTestHelpers.Fakes.UserAccount;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Threading.Tasks;
using AccountManagement.Domain.UserAccounts.Features;
using static TestFixture;
using SharedKernel.Exceptions;
using AccountManagement.SharedTestHelpers.Fakes.User;
using Domain.MonetaryAmounts;

public class OpenUserAccountCommandTests : TestBase
{
    [Test]
    public async Task can_add_new_useraccount_to_db()
    {
        // Arrange
        var fakeUserAccountOne = new FakeUserAccountForCreationDto().Generate();

        // Act
        var command = new OpenUserAccount.Command(fakeUserAccountOne);
        var userAccountReturned = await SendAsync(command);
        var userAccountCreated = await ExecuteDbContextAsync(db => db.UserAccounts
            .FirstOrDefaultAsync(u => u.Id == userAccountReturned.Id));

        // Assert
        userAccountReturned.Balance.Should().Be(MonetaryAmount.Of(fakeUserAccountOne.Balance).Amount);
        userAccountCreated.Balance.Should().Be(MonetaryAmount.Of(fakeUserAccountOne.Balance));
    }
}