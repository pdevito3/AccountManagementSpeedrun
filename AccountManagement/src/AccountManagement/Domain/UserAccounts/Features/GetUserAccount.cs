namespace AccountManagement.Domain.UserAccounts.Features;

using AccountManagement.Domain.UserAccounts.Dtos;
using AccountManagement.Domain.UserAccounts.Services;
using SharedKernel.Exceptions;
using AccountManagement.Domain;
using HeimGuard;
using MapsterMapper;
using MediatR;

public static class GetUserAccount
{
    public sealed class Query : IRequest<UserAccountDto>
    {
        public readonly Guid Id;

        public Query(Guid id)
        {
            Id = id;
        }
    }

    public sealed class Handler : IRequestHandler<Query, UserAccountDto>
    {
        private readonly IUserAccountRepository _userAccountRepository;
        private readonly IMapper _mapper;
        private readonly IHeimGuardClient _heimGuard;

        public Handler(IUserAccountRepository userAccountRepository, IMapper mapper, IHeimGuardClient heimGuard)
        {
            _mapper = mapper;
            _userAccountRepository = userAccountRepository;
            _heimGuard = heimGuard;
        }

        public async Task<UserAccountDto> Handle(Query request, CancellationToken cancellationToken)
        {
            await _heimGuard.MustHavePermission<ForbiddenAccessException>(Permissions.CanReadBankingSystem);

            var result = await _userAccountRepository.GetById(request.Id, cancellationToken: cancellationToken);
            return _mapper.Map<UserAccountDto>(result);
        }
    }
}