namespace AccountManagement.Domain.UserAccounts.DomainEvents;

public sealed class UserAccountCreated : DomainEvent
{
    public UserAccount UserAccount { get; set; } 
}
            