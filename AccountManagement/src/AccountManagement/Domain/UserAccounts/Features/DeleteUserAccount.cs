namespace AccountManagement.Domain.UserAccounts.Features;

using AccountManagement.Domain.UserAccounts.Services;
using AccountManagement.Services;
using SharedKernel.Exceptions;
using AccountManagement.Domain;
using HeimGuard;
using MediatR;

public static class DeleteUserAccount
{
    public sealed class Command : IRequest<bool>
    {
        public readonly Guid Id;

        public Command(Guid userAccount)
        {
            Id = userAccount;
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
            await _heimGuard.MustHavePermission<ForbiddenAccessException>(Permissions.CanDeleteUserAccount);

            var recordToDelete = await _userAccountRepository.GetById(request.Id, cancellationToken: cancellationToken);

            _userAccountRepository.Remove(recordToDelete);
            return await _unitOfWork.CommitChanges(cancellationToken) >= 1;
        }
    }
}