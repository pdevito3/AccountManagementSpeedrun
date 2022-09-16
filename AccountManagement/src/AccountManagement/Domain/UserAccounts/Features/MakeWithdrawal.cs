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

public static class MakeWithdrawal
{
    public sealed class Command : IRequest<bool>
    {
        public readonly Guid Id;
        public readonly decimal DepositAmount;

        public Command(Guid userAccount, decimal depositAmount)
        {
            Id = userAccount;
            DepositAmount = depositAmount;
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
            await _heimGuard.MustHavePermission<ForbiddenAccessException>(Permissions.CanMakeWithdrawl);

            var userAccountToUpdate = await _userAccountRepository.GetById(request.Id, cancellationToken: cancellationToken);

            userAccountToUpdate.Withdraw(request.DepositAmount);
            _userAccountRepository.Update(userAccountToUpdate);
            return await _unitOfWork.CommitChanges(cancellationToken) >= 1;
        }
    }
}