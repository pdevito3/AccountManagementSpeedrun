namespace AccountManagement.Domain.RolePermissions.Features;

using AccountManagement.Domain.RolePermissions.Services;
using AccountManagement.Domain.RolePermissions;
using AccountManagement.Domain.RolePermissions.Dtos;
using AccountManagement.Services;
using SharedKernel.Exceptions;
using AccountManagement.Domain;
using HeimGuard;
using MapsterMapper;
using MediatR;

public static class AddRolePermission
{
    public sealed class Command : IRequest<RolePermissionDto>
    {
        public readonly RolePermissionForCreationDto RolePermissionToAdd;

        public Command(RolePermissionForCreationDto rolePermissionToAdd)
        {
            RolePermissionToAdd = rolePermissionToAdd;
        }
    }

    public sealed class Handler : IRequestHandler<Command, RolePermissionDto>
    {
        private readonly IRolePermissionRepository _rolePermissionRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHeimGuardClient _heimGuard;

        public Handler(IRolePermissionRepository rolePermissionRepository, IUnitOfWork unitOfWork, IMapper mapper, IHeimGuardClient heimGuard)
        {
            _mapper = mapper;
            _rolePermissionRepository = rolePermissionRepository;
            _unitOfWork = unitOfWork;
            _heimGuard = heimGuard;
        }

        public async Task<RolePermissionDto> Handle(Command request, CancellationToken cancellationToken)
        {
            await _heimGuard.MustHavePermission<ForbiddenAccessException>(Permissions.CanAddRolePermission);

            var rolePermission = RolePermission.Create(request.RolePermissionToAdd);
            await _rolePermissionRepository.Add(rolePermission, cancellationToken);

            await _unitOfWork.CommitChanges(cancellationToken);

            var rolePermissionAdded = await _rolePermissionRepository.GetById(rolePermission.Id, cancellationToken: cancellationToken);
            return _mapper.Map<RolePermissionDto>(rolePermissionAdded);
        }
    }
}