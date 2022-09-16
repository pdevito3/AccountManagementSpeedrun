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
            // .Map(x => x.Balance, y => new MonetaryAmount(y.Balance));
            config.NewConfig<UserAccount, UserAccountDto>();
                // .MapWith(x => x.Balance.Amount);
            // .Map(x => x.Balance, y => y.Balance.Amount);
        config.NewConfig<UserAccountForCreationDto, UserAccount>()
            .TwoWays();
        config.NewConfig<UserAccountForUpdateDto, UserAccount>()
            .TwoWays();
    }
}