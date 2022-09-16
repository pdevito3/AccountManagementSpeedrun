namespace AccountManagement.IntegrationTests.FeatureTests.Users;

using AccountManagement.SharedTestHelpers.Fakes.User;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Threading.Tasks;
using AccountManagement.Domain.Users.Features;
using static TestFixture;
using SharedKernel.Exceptions;

public class AddUserCommandTests : TestBase
{
    [Test]
    public async Task can_add_new_user_to_db()
    {
        // Arrange
        var fakeUserOne = new FakeUserForCreationDto().Generate();

        // Act
        var command = new AddUser.Command(fakeUserOne);
        var userReturned = await SendAsync(command);
        var userCreated = await ExecuteDbContextAsync(db => db.Users
            .FirstOrDefaultAsync(u => u.Id == userReturned.Id));

        // Assert
        userReturned.Should().BeEquivalentTo(fakeUserOne, options =>
            options.ExcludingMissingMembers()
                .Excluding(x => x.Email));
        userReturned.Email.Should().Be(fakeUserOne.Email);
        userCreated.Should().BeEquivalentTo(fakeUserOne, options =>
            options.ExcludingMissingMembers()
                .Excluding(x => x.Email));
        userCreated.Email.Value.Should().Be(fakeUserOne.Email);
    }
}