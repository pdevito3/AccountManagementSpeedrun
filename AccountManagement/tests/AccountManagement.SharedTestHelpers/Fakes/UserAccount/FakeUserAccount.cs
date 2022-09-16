namespace AccountManagement.SharedTestHelpers.Fakes.UserAccount;

using AutoBogus;
using AccountManagement.Domain.UserAccounts;
using AccountManagement.Domain.UserAccounts.Dtos;

public class FakeUserAccount
{
    public static UserAccount Generate(UserAccountForCreationDto userAccountForCreationDto)
    {
        return UserAccount.Open(userAccountForCreationDto);
    }

    public static UserAccount Generate()
    {
        return UserAccount.Open(new FakeUserAccountForCreationDto().Generate());
    }
}