namespace AccountManagement.Domain.Users.Mappings;

using AccountManagement.Domain.Users.Dtos;
using AccountManagement.Domain.Users;
using Mapster;

public sealed class UserMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<UserDto, User>()
            .TwoWays();
        config.NewConfig<UserForCreationDto, User>()
            .TwoWays();
        config.NewConfig<UserForUpdateDto, User>()
            .TwoWays();
    }
}