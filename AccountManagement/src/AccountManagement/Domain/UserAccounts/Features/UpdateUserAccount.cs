namespace AccountManagement.Domain.UserAccounts.Features;

using AccountManagement.Domain.UserAccounts;
using AccountManagement.Domain.UserAccounts.Dtos;
using AccountManagement.Domain.UserAccounts.Validators;
using AccountManagement.Domain.UserAccounts.Services;
using AccountManagement.Services;
using SharedKernel.Exceptions;
using AccountManagement.Domain;
using HeimGuard;
using MapsterMapper;
using MediatR;

public static class UpdateUserAccount
{
    public sealed class Command : IRequest<bool>
    {
        public readonly Guid Id;
        public readonly UserAccountForUpdateDto UserAccountToUpdate;

        public Command(Guid userAccount, UserAccountForUpdateDto newUserAccountData)
        {
            Id = userAccount;
            UserAccountToUpdate = newUserAccountData;
        }
    }

    public sealed class Handler : IRequestHandler<Command, bool>
    {
        private readonly IUserAccountRepository _userAccountRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHeimGuardClient _heimGuard;

        public Handler(IUserAccountRepository userAccountRepository, IUnitOfWork unitOfWork, IHeimGuardClient heimGuard)
        {
            _userAccountRepository = userAccountRepository;
            _unitOfWork = unitOfWork;
            _heimGuard = heimGuard;
        }

        public async Task<bool> Handle(Command request, CancellationToken cancellationToken)
        {
            await _heimGuard.MustHavePermission<ForbiddenAccessException>(Permissions.CanUpdateUserAccount);

            var userAccountToUpdate = await _userAccountRepository.GetById(request.Id, cancellationToken: cancellationToken);

            userAccountToUpdate.Update(request.UserAccountToUpdate);
            _userAccountRepository.Update(userAccountToUpdate);
            return await _unitOfWork.CommitChanges(cancellationToken) >= 1;
        }
    }
}