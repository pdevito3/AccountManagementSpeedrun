namespace AccountManagement.Domain.UserAccounts.DomainEvents;

public sealed class UserAccountUpdated : DomainEvent
{
    public Guid Id { get; set; } 
}
            