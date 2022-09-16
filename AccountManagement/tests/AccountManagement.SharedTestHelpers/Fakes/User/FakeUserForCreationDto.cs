namespace AccountManagement.SharedTestHelpers.Fakes.User;

using AutoBogus;
using AccountManagement.Domain;
using AccountManagement.Domain.Users.Dtos;
using AccountManagement.Domain.Roles;

public class FakeUserForCreationDto : AutoFaker<UserForCreationDto>
{
    public FakeUserForCreationDto()
    {
        RuleFor(u => u.Email, f => f.Person.Email);
    }
}