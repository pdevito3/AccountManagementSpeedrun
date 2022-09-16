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

public class UserAccount : BaseEntity
{
    [Sieve(CanFilter = true, CanSort = true)]
    public virtual MonetaryAmount Balance { get; private set; }

    [JsonIgnore]
    [IgnoreDataMember]
    public virtual User User { get; private set; }


    public static UserAccount Create(UserAccountForCreationDto userAccountForCreationDto)
    {
        new UserAccountForCreationDtoValidator().ValidateAndThrow(userAccountForCreationDto);

        var newUserAccount = new UserAccount();

        newUserAccount.Balance = new MonetaryAmount(userAccountForCreationDto.Balance);

        newUserAccount.QueueDomainEvent(new UserAccountCreated(){ UserAccount = newUserAccount });
        
        return newUserAccount;
    }

    public void Deposit(decimal depositAmount)
    {
        Balance +=  new MonetaryAmount(depositAmount);
        QueueDomainEvent(new UserAccountUpdated(){ Id = Id });
    }

    public void Withdraw(decimal depositAmount)
    {
        Balance -=  new MonetaryAmount(depositAmount);
        QueueDomainEvent(new UserAccountUpdated(){ Id = Id });
    }
    
    protected UserAccount() { } // For EF + Mocking
}