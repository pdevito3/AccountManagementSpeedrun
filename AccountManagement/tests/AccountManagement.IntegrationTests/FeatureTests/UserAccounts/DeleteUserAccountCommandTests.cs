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

public class DeleteUserAccountCommandTests : TestBase
{
    [Test]
    public async Task can_delete_useraccount_from_db()
    {
        // Arrange
        var fakeUserAccountOne = FakeUserAccount.Generate(new FakeUserAccountForCreationDto().Generate());
        await InsertAsync(fakeUserAccountOne);
        var userAccount = await ExecuteDbContextAsync(db => db.UserAccounts
            .FirstOrDefaultAsync(u => u.Id == fakeUserAccountOne.Id));

        // Act
        var command = new DeleteUserAccount.Command(userAccount.Id);
        await SendAsync(command);
        var userAccountResponse = await ExecuteDbContextAsync(db => db.UserAccounts.CountAsync(u => u.Id == userAccount.Id));

        // Assert
        userAccountResponse.Should().Be(0);
    }

    [Test]
    public async Task delete_useraccount_throws_notfoundexception_when_record_does_not_exist()
    {
        // Arrange
        var badId = Guid.NewGuid();

        // Act
        var command = new DeleteUserAccount.Command(badId);
        Func<Task> act = () => SendAsync(command);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Test]
    public async Task can_softdelete_useraccount_from_db()
    {
        // Arrange
        var fakeUserAccountOne = FakeUserAccount.Generate(new FakeUserAccountForCreationDto().Generate());
        await InsertAsync(fakeUserAccountOne);
        var userAccount = await ExecuteDbContextAsync(db => db.UserAccounts
            .FirstOrDefaultAsync(u => u.Id == fakeUserAccountOne.Id));

        // Act
        var command = new DeleteUserAccount.Command(userAccount.Id);
        await SendAsync(command);
        var deletedUserAccount = await ExecuteDbContextAsync(db => db.UserAccounts
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(x => x.Id == userAccount.Id));

        // Assert
        deletedUserAccount?.IsDeleted.Should().BeTrue();
    }
}