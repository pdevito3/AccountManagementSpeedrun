namespace AccountManagement.Domain.UserAccounts.Mappings;

using AccountManagement.Domain.UserAccounts.Dtos;
using AccountManagement.Domain.UserAccounts;
using Mapster;
using MonetaryAmounts;

public sealed class UserAccountMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<UserAccountDto, UserAccount>();
        config.NewConfig<UserAccount, UserAccountDto>();
        config.NewConfig<UserAccountForCreationDto, UserAccount>()
            .TwoWays();
    }
}