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


public class UserAccount : BaseEntity
{
    [Sieve(CanFilter = true, CanSort = true)]
    public virtual decimal Balance { get; private set; }

    [JsonIgnore]
    [IgnoreDataMember]
    public virtual User User { get; private set; }


    public static UserAccount Create(UserAccountForCreationDto userAccountForCreationDto)
    {
        new UserAccountForCreationDtoValidator().ValidateAndThrow(userAccountForCreationDto);

        var newUserAccount = new UserAccount();

        newUserAccount.Balance = userAccountForCreationDto.Balance;

        newUserAccount.QueueDomainEvent(new UserAccountCreated(){ UserAccount = newUserAccount });
        
        return newUserAccount;
    }

    public void Update(UserAccountForUpdateDto userAccountForUpdateDto)
    {
        new UserAccountForUpdateDtoValidator().ValidateAndThrow(userAccountForUpdateDto);

        Balance = userAccountForUpdateDto.Balance;

        QueueDomainEvent(new UserAccountUpdated(){ Id = Id });
    }
    
    protected UserAccount() { } // For EF + Mocking
}