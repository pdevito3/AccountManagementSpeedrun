namespace AccountManagement.Domain.UserAccounts;

using SharedKernel.Exceptions;
using AccountManagement.Domain.UserAccounts.Dtos;
using AccountManagement.Domain.UserAccounts.Validators;
using AccountManagement.Domain.UserAccounts.DomainEvents;
using FluentValidation;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Sieve.Attributes;
using AccountManagement.Domain.Users;
using MonetaryAmounts;
using Percentages;

public class UserAccount : BaseEntity
{
    [Sieve(CanFilter = true, CanSort = true)]
    public virtual MonetaryAmount Balance { get; private set; }

    [JsonIgnore]
    [IgnoreDataMember]
    public virtual User User { get; private set; }


    public static UserAccount Open(UserAccountForCreationDto userAccountForCreationDto)
    {
        new UserAccountForCreationDtoValidator().ValidateAndThrow(userAccountForCreationDto);

        var newUserAccount = new UserAccount();

        newUserAccount.Balance = new MonetaryAmount(userAccountForCreationDto.Balance);

        newUserAccount.QueueDomainEvent(new UserAccountCreated(){ UserAccount = newUserAccount });
        
        return newUserAccount;
    }

    public void Deposit(decimal depositAmount)
    {
        var newBalance =  Balance.Add(MonetaryAmount.Of(depositAmount));
        if (newBalance > MonetaryAmount.Of(10000))
            throw new FluentValidation.ValidationException(
                "You can not deposit more than $10,000 per transaction.");

        Balance = newBalance;
        
        QueueDomainEvent(new UserAccountUpdated(){ Id = Id });
    }

    public void Withdraw(decimal depositAmount)
    {
        var newBalance =  Balance.Subtract(MonetaryAmount.Of(depositAmount));
        if (newBalance < Balance.MultiplyByPercent(new Percent(10)))
            throw new FluentValidation.ValidationException(
                "You can not withdraw more than 90% of your balance in one transaction.");

        Balance = newBalance;
        QueueDomainEvent(new UserAccountUpdated(){ Id = Id });
    }
    
    protected UserAccount() { } // For EF + Mocking
}