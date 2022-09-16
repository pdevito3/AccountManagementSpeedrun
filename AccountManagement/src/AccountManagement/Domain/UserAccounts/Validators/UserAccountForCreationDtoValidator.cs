namespace AccountManagement.Domain.UserAccounts.Validators;

using AccountManagement.Domain.UserAccounts.Dtos;
using FluentValidation;
using MonetaryAmounts;

public sealed class UserAccountForCreationDtoValidator: UserAccountForManipulationDtoValidator<UserAccountForCreationDto>
{
    public UserAccountForCreationDtoValidator()
    {
        RuleFor(x => x.Balance)
            .Must(BeAtLeastOneHundredDollars)
            .WithMessage("Starting deposit must be at least $100");
    }
    private static bool BeAtLeastOneHundredDollars(decimal balance)
    {
        var one = new MonetaryAmount(balance);
        var two = new MonetaryAmount(100);
        return new MonetaryAmount(balance) >= new MonetaryAmount(100);
    }
}