namespace AccountManagement.Domain.UserAccounts.Mappings;

using AccountManagement.Domain.UserAccounts.Dtos;
using AccountManagement.Domain.UserAccounts;
using Mapster;

public sealed class UserAccountMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<UserAccountDto, UserAccount>()
            .TwoWays();
        config.NewConfig<UserAccountForCreationDto, UserAccount>()
            .TwoWays();
        config.NewConfig<UserAccountForUpdateDto, UserAccount>()
            .TwoWays();
    }
}