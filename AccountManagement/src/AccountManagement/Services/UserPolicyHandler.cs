namespace AccountManagement.Services;

using Domain.Roles;
using Domain.Users.Dtos;
using Domain.Users.Features;
using Domain.Users.Services;
using AccountManagement.Domain.RolePermissions.Services;
using AccountManagement.Domain;
using SharedKernel.Exceptions;
using HeimGuard;
using MediatR;
using Microsoft.EntityFrameworkCore;

public sealed class UserPolicyHandler : IUserPolicyHandler
{
    private readonly IRolePermissionRepository _rolePermissionRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IUserRepository _userRepository;
    private readonly IMediator _mediator;

    public UserPolicyHandler(IRolePermissionRepository rolePermissionRepository, ICurrentUserService currentUserService, IUserRepository userRepository, IMediator mediator)
    {
        _rolePermissionRepository = rolePermissionRepository;
        _currentUserService = currentUserService;
        _userRepository = userRepository;
        _mediator = mediator;
    }
    
    public async Task<IEnumerable<string>> GetUserPermissions()
    {
        var claimsPrincipal = _currentUserService.User;
        if (claimsPrincipal == null) throw new ArgumentNullException(nameof(claimsPrincipal));
        
        var nameIdentifier = _currentUserService.UserId;
        var usersExist = _userRepository.Query().Any();
        
        if (!usersExist)
            await SeedRootUser(nameIdentifier);

        var roles = GetRoles(nameIdentifier);

        if (roles.Length == 0)
            throw new NoRolesAssignedException();

        // super admins can do everything
        if(roles.Contains(Role.SuperAdmin().Value))
            return Permissions.List();

        var permissions = await _rolePermissionRepository.Query()
            .Where(rp => roles.Contains(rp.Role))
            .Select(rp => rp.Permission)
            .Distinct()
            .ToArrayAsync();

        return await Task.FromResult(permissions);
    }

    private async Task SeedRootUser(string userId)
    {
        var rootUser = new UserForCreationDto()
        {
            Username = _currentUserService.Username,
            Email = _currentUserService.Email,
            FirstName = _currentUserService.FirstName,
            LastName = _currentUserService.LastName,
            Identifier = userId
        };

        var userCommand = new AddUser.Command(rootUser, true);
        var createdUser = await _mediator.Send(userCommand);

        var roleCommand = new AddUserRole.Command(createdUser.Id, Role.SuperAdmin().Value, true);
        await _mediator.Send(roleCommand);
        
    }

    private string[] GetRoles(string nameIdentifier)
    {
        if(!string.IsNullOrEmpty(nameIdentifier))
            return _userRepository.GetRolesByUserIdentifier(nameIdentifier).ToArray();

        return Array.Empty<string>();
    }
}