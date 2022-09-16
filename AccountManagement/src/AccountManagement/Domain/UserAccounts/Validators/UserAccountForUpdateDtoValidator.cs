namespace AccountManagement.Domain.UserAccounts.Validators;

using AccountManagement.Domain.UserAccounts.Dtos;
using FluentValidation;

public sealed class UserAccountForUpdateDtoValidator: UserAccountForManipulationDtoValidator<UserAccountForUpdateDto>
{
    public UserAccountForUpdateDtoValidator()
    {
        // add fluent validation rules that should only be run on update operations here
        //https://fluentvalidation.net/
    }
}