namespace AccountManagement.Domain.Users.Features;

using AccountManagement.Domain.Users.Services;
using AccountManagement.Domain.Users;
using AccountManagement.Domain.Users.Dtos;
using AccountManagement.Services;
using SharedKernel.Exceptions;
using HeimGuard;
using MediatR;
using Roles;

public static class AddUserRole
{
    public sealed class Command : IRequest<bool>
    {
        public readonly Guid UserId;
        public readonly string Role;
        public readonly bool SkipPermissions;

        public Command(Guid userId, string role, bool skipPermissions = false)
        {
            UserId = userId;
            Role = role;
            SkipPermissions = skipPermissions;
        }
    }

    public sealed class Handler : IRequestHandler<Command, bool>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHeimGuardClient _heimGuard;

        public Handler(IUserRepository userRepository, IUnitOfWork unitOfWork, IHeimGuardClient heimGuard)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _heimGuard = heimGuard;
        }

        public async Task<bool> Handle(Command request, CancellationToken cancellationToken)
        {
            if(!request.SkipPermissions)
                await _heimGuard.MustHavePermission<ForbiddenAccessException>(Permissions.CanAddUserRole);
            
            var user = await _userRepository.GetById(request.UserId, true, cancellationToken);

            var roleToAdd = user.AddRole(new Role(request.Role));
            await _userRepository.AddRole(roleToAdd, cancellationToken);
            await _unitOfWork.CommitChanges(cancellationToken);

            return true;
        }
    }
}