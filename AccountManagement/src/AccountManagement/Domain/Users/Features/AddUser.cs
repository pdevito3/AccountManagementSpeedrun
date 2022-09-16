namespace AccountManagement.Domain.Users.Features;

using AccountManagement.Domain.Users.Services;
using AccountManagement.Domain.Users;
using AccountManagement.Domain.Users.Dtos;
using AccountManagement.Services;
using SharedKernel.Exceptions;
using AccountManagement.Domain;
using HeimGuard;
using MapsterMapper;
using MediatR;

public static class AddUser
{
    public sealed class Command : IRequest<UserDto>
    {
        public readonly UserForCreationDto UserToAdd;
        public readonly bool SkipPermissions;

        public Command(UserForCreationDto userToAdd, bool skipPermissions = false)
        {
            UserToAdd = userToAdd;
            SkipPermissions = skipPermissions;
        }
    }

    public sealed class Handler : IRequestHandler<Command, UserDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHeimGuardClient _heimGuard;

        public Handler(IUserRepository userRepository, IUnitOfWork unitOfWork, IMapper mapper, IHeimGuardClient heimGuard)
        {
            _mapper = mapper;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _heimGuard = heimGuard;
        }

        public async Task<UserDto> Handle(Command request, CancellationToken cancellationToken)
        {
            if(!request.SkipPermissions)
                await _heimGuard.MustHavePermission<ForbiddenAccessException>(Permissions.CanAddUser);

            var user = User.Create(request.UserToAdd);
            await _userRepository.Add(user, cancellationToken);

            await _unitOfWork.CommitChanges(cancellationToken);

            var userAdded = await _userRepository.GetById(user.Id, cancellationToken: cancellationToken);
            return _mapper.Map<UserDto>(userAdded);
        }
    }
}
