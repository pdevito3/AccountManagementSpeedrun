namespace AccountManagement.UnitTests.UnitTests.Domain.UserAccounts;

using AccountManagement.SharedTestHelpers.Fakes.UserAccount;
using AccountManagement.Domain.UserAccounts;
using AccountManagement.Domain.UserAccounts.DomainEvents;
using Bogus;
using FluentAssertions;
using NUnit.Framework;

[Parallelizable]
public class UpdateUserAccountTests
{
    private readonly Faker _faker;

    public UpdateUserAccountTests()
    {
        _faker = new Faker();
    }
    
    [Test]
    public void can_update_userAccount()
    {
        // Arrange
        var fakeUserAccount = FakeUserAccount.Generate();
        var updatedUserAccount = new FakeUserAccountForUpdateDto().Generate();
        
        // Act
        fakeUserAccount.Update(updatedUserAccount);

        // Assert
        fakeUserAccount.Should().BeEquivalentTo(updatedUserAccount, options =>
            options.ExcludingMissingMembers());
    }
    
    [Test]
    public void queue_domain_event_on_update()
    {
        // Arrange
        var fakeUserAccount = FakeUserAccount.Generate();
        var updatedUserAccount = new FakeUserAccountForUpdateDto().Generate();
        fakeUserAccount.DomainEvents.Clear();
        
        // Act
        fakeUserAccount.Update(updatedUserAccount);

        // Assert
        fakeUserAccount.DomainEvents.Count.Should().Be(1);
        fakeUserAccount.DomainEvents.FirstOrDefault().Should().BeOfType(typeof(UserAccountUpdated));
    }
}