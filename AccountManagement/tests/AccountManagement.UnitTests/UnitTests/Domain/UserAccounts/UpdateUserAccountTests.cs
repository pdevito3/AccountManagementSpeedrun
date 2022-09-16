namespace AccountManagement.UnitTests.UnitTests.Domain.UserAccounts;

using AccountManagement.Domain.MonetaryAmounts;
using AccountManagement.Domain.Percentages;
using AccountManagement.SharedTestHelpers.Fakes.UserAccount;
using AccountManagement.Domain.UserAccounts;
using AccountManagement.Domain.UserAccounts.DomainEvents;
using AccountManagement.Domain.UserAccounts.Dtos;
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
    public void can_deposit_to_userAccount()
    {
        // Arrange
        var fakeUserAccount = FakeUserAccount.Generate();
        var startingAmount = fakeUserAccount.Balance;
        
        // Act
        fakeUserAccount.Deposit(100);

        // Assert
        var expectedAmount = startingAmount.Add(new MonetaryAmount(100));
        fakeUserAccount.Balance.Amount.Should().Be(expectedAmount.Amount);
    }
    
    [Test]
    public void can_withdraw_from_userAccount()
    {
        // Arrange
        var fakeUserAccount = FakeUserAccount.Generate();
        var startingAmount = fakeUserAccount.Balance;
        
        // Act
        fakeUserAccount.Withdraw(1);

        // Assert
        var expectedAmount = startingAmount.Subtract(new MonetaryAmount(1));
        fakeUserAccount.Balance.Amount.Should().Be(expectedAmount.Amount);
    }
    
    [Test]
    public void can_not_open_account_with_less_than_100_dollars()
    {
        // Arrange
        var fakeUserAccount = new FakeUserAccountForCreationDto()
            .RuleFor(x => x.Balance, 5)
            .Generate();
        
        // Act
        var act = () => UserAccount.Open(fakeUserAccount);

        // Assert
        act.Should().Throw<FluentValidation.ValidationException>();
    }
    
    [Test]
    public void can_not_withdraw_more_than_90_percent()
    {
        // Arrange
        var fakeUserAccount = FakeUserAccount.Generate();
        var startingAmount = fakeUserAccount.Balance;
        var withdrawalAmount = startingAmount.MultiplyByPercent(new Percent(91));
        
        // Act
        var act = () => fakeUserAccount.Withdraw(withdrawalAmount.Amount);

        // Assert
        act.Should().Throw<FluentValidation.ValidationException>();
    }
    
    [Test]
    public void queue_domain_event_on_update()
    {
        // Arrange
        var fakeUserAccount = FakeUserAccount.Generate();
        fakeUserAccount.DomainEvents.Clear();
        
        // Act
        fakeUserAccount.Deposit(100);

        // Assert
        fakeUserAccount.DomainEvents.Count.Should().Be(1);
        fakeUserAccount.DomainEvents.FirstOrDefault().Should().BeOfType(typeof(UserAccountUpdated));
    }
}