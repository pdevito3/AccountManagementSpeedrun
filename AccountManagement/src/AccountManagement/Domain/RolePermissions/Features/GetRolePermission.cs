namespace AccountManagement.Domain.RolePermissions.Features;

using AccountManagement.Domain.RolePermissions.Dtos;
using AccountManagement.Domain.RolePermissions.Services;
using SharedKernel.Exceptions;
using AccountManagement.Domain;
using HeimGuard;
using MapsterMapper;
using MediatR;

public static class GetRolePermission
{
    public sealed class Query : IRequest<RolePermissionDto>
    {
        public readonly Guid Id;

        public Query(Guid id)
        {
            Id = id;
        }
    }

    public sealed class Handler : IRequestHandler<Query, RolePermissionDto>
    {
        private readonly IRolePermissionRepository _rolePermissionRepository;
        private readonly IMapper _mapper;
        private readonly IHeimGuardClient _heimGuard;

        public Handler(IRolePermissionRepository rolePermissionRepository, IMapper mapper, IHeimGuardClient heimGuard)
        {
            _mapper = mapper;
            _rolePermissionRepository = rolePermissionRepository;
            _heimGuard = heimGuard;
        }

        public async Task<RolePermissionDto> Handle(Query request, CancellationToken cancellationToken)
        {
            await _heimGuard.MustHavePermission<ForbiddenAccessException>(Permissions.CanReadRolePermissions);

            var result = await _rolePermissionRepository.GetById(request.Id, cancellationToken: cancellationToken);
            return _mapper.Map<RolePermissionDto>(result);
        }
    }
}