namespace AccountManagement.SharedTestHelpers.Fakes.UserAccount;

using AutoBogus;
using AccountManagement.Domain.UserAccounts;
using AccountManagement.Domain.UserAccounts.Dtos;

// or replace 'AutoFaker' with 'Faker' along with your own rules if you don't want all fields to be auto faked
public class FakeUserAccountForUpdateDto : AutoFaker<UserAccountForUpdateDto>
{
    public FakeUserAccountForUpdateDto()
    {
        // if you want default values on any of your properties (e.g. an int between a certain range or a date always in the past), you can add `RuleFor` lines describing those defaults
        //RuleFor(u => u.ExampleIntProperty, u => u.Random.Number(50, 100000));
        //RuleFor(u => u.ExampleDateProperty, u => u.Date.Past());
    }
}