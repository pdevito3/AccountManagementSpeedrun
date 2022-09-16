namespace AccountManagement.Domain.UserAccounts.Features;

using AccountManagement.Domain.UserAccounts.Dtos;
using AccountManagement.Domain.UserAccounts.Services;
using AccountManagement.Wrappers;
using SharedKernel.Exceptions;
using AccountManagement.Domain;
using HeimGuard;
using MapsterMapper;
using Mapster;
using MediatR;
using Sieve.Models;
using Sieve.Services;

public static class GetUserAccountList
{
    public sealed class Query : IRequest<PagedList<UserAccountDto>>
    {
        public readonly UserAccountParametersDto QueryParameters;

        public Query(UserAccountParametersDto queryParameters)
        {
            QueryParameters = queryParameters;
        }
    }

    public sealed class Handler : IRequestHandler<Query, PagedList<UserAccountDto>>
    {
        private readonly IUserAccountRepository _userAccountRepository;
        private readonly SieveProcessor _sieveProcessor;
        private readonly IMapper _mapper;
        private readonly IHeimGuardClient _heimGuard;

        public Handler(IUserAccountRepository userAccountRepository, IMapper mapper, SieveProcessor sieveProcessor, IHeimGuardClient heimGuard)
        {
            _mapper = mapper;
            _userAccountRepository = userAccountRepository;
            _sieveProcessor = sieveProcessor;
            _heimGuard = heimGuard;
        }

        public async Task<PagedList<UserAccountDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            await _heimGuard.MustHavePermission<ForbiddenAccessException>(Permissions.CanReadBankingSystem);

            var collection = _userAccountRepository.Query();

            var sieveModel = new SieveModel
            {
                Sorts = request.QueryParameters.SortOrder ?? "-CreatedOn",
                Filters = request.QueryParameters.Filters
            };

            var appliedCollection = _sieveProcessor.Apply(sieveModel, collection);
            var dtoCollection = appliedCollection
                .ProjectToType<UserAccountDto>();

            return await PagedList<UserAccountDto>.CreateAsync(dtoCollection,
                request.QueryParameters.PageNumber,
                request.QueryParameters.PageSize,
                cancellationToken);
        }
    }
}