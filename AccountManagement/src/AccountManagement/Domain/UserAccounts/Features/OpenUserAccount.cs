namespace AccountManagement.Domain.UserAccounts.Features;

using AccountManagement.Domain.UserAccounts.Services;
using AccountManagement.Domain.UserAccounts;
using AccountManagement.Domain.UserAccounts.Dtos;
using AccountManagement.Services;
using SharedKernel.Exceptions;
using AccountManagement.Domain;
using HeimGuard;
using MapsterMapper;
using MediatR;

public static class OpenUserAccount
{
    public sealed class Command : IRequest<UserAccountDto>
    {
        public readonly UserAccountForCreationDto UserAccountToAdd;

        public Command(UserAccountForCreationDto userAccountToAdd)
        {
            UserAccountToAdd = userAccountToAdd;
        }
    }

    public sealed class Handler : IRequestHandler<Command, UserAccountDto>
    {
        private readonly IUserAccountRepository _userAccountRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHeimGuardClient _heimGuard;

        public Handler(IUserAccountRepository userAccountRepository, IUnitOfWork unitOfWork, IMapper mapper, IHeimGuardClient heimGuard)
        {
            _mapper = mapper;
            _userAccountRepository = userAccountRepository;
            _unitOfWork = unitOfWork;
            _heimGuard = heimGuard;
        }

        public async Task<UserAccountDto> Handle(Command request, CancellationToken cancellationToken)
        {
            await _heimGuard.MustHavePermission<ForbiddenAccessException>(Permissions.CanAddUserAccount);

            var userAccount = UserAccount.Open(request.UserAccountToAdd);
            await _userAccountRepository.Add(userAccount, cancellationToken);

            await _unitOfWork.CommitChanges(cancellationToken);

            var userAccountAdded = await _userAccountRepository.GetById(userAccount.Id, cancellationToken: cancellationToken);
            return _mapper.Map<UserAccountDto>(userAccountAdded);
        }
    }
}