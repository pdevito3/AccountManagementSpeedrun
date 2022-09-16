namespace AccountManagement.Domain.UserAccounts.Validators;

using AccountManagement.Domain.UserAccounts.Dtos;
using FluentValidation;

public sealed class UserAccountForCreationDtoValidator: UserAccountForManipulationDtoValidator<UserAccountForCreationDto>
{
    public UserAccountForCreationDtoValidator()
    {
        // add fluent validation rules that should only be run on creation operations here
        //https://fluentvalidation.net/
    }
}