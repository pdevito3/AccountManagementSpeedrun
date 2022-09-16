namespace AccountManagement.UnitTests.UnitTests.Domain.UserAccounts;

using AccountManagement.SharedTestHelpers.Fakes.UserAccount;
using AccountManagement.Domain.UserAccounts;
using AccountManagement.Domain.UserAccounts.DomainEvents;
using Bogus;
using FluentAssertions;
using NUnit.Framework;

[Parallelizable]
public class CreateUserAccountTests
{
    private readonly Faker _faker;

    public CreateUserAccountTests()
    {
        _faker = new Faker();
    }
    
    [Test]
    public void can_create_valid_userAccount()
    {
        // Arrange + Act
        var fakeUserAccount = FakeUserAccount.Generate();

        // Assert
        fakeUserAccount.Should().NotBeNull();
    }

    [Test]
    public void queue_domain_event_on_create()
    {
        // Arrange + Act
        var fakeUserAccount = FakeUserAccount.Generate();

        // Assert
        fakeUserAccount.DomainEvents.Count.Should().Be(1);
        fakeUserAccount.DomainEvents.FirstOrDefault().Should().BeOfType(typeof(UserAccountCreated));
    }
}