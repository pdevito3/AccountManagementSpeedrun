namespace AccountManagement.IntegrationTests.FeatureTests.UserAccounts;

using AccountManagement.SharedTestHelpers.Fakes.UserAccount;
using AccountManagement.Domain.UserAccounts.Dtos;
using SharedKernel.Exceptions;
using AccountManagement.Domain.UserAccounts.Features;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Threading.Tasks;
using static TestFixture;
using AccountManagement.SharedTestHelpers.Fakes.User;

public class UpdateUserAccountCommandTests : TestBase
{
    [Test]
    public async Task can_update_existing_useraccount_in_db()
    {
        // Arrange
        var fakeUserAccountOne = FakeUserAccount.Generate(new FakeUserAccountForCreationDto().Generate());
        var updatedUserAccountDto = new FakeUserAccountForUpdateDto().Generate();
        await InsertAsync(fakeUserAccountOne);

        var userAccount = await ExecuteDbContextAsync(db => db.UserAccounts
            .FirstOrDefaultAsync(u => u.Id == fakeUserAccountOne.Id));
        var id = userAccount.Id;

        // Act
        var command = new UpdateUserAccount.Command(id, updatedUserAccountDto);
        await SendAsync(command);
        var updatedUserAccount = await ExecuteDbContextAsync(db => db.UserAccounts.FirstOrDefaultAsync(u => u.Id == id));

        // Assert
        updatedUserAccount.Should().BeEquivalentTo(updatedUserAccountDto, options =>
            options.ExcludingMissingMembers());
    }
}