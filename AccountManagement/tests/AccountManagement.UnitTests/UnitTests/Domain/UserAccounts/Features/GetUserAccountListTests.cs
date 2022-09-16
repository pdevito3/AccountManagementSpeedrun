namespace AccountManagement.UnitTests.UnitTests.Domain.UserAccounts.Features;

using AccountManagement.SharedTestHelpers.Fakes.UserAccount;
using AccountManagement.Domain.UserAccounts;
using AccountManagement.Domain.UserAccounts.Dtos;
using AccountManagement.Domain.UserAccounts.Mappings;
using AccountManagement.Domain.UserAccounts.Features;
using AccountManagement.Domain.UserAccounts.Services;
using MapsterMapper;
using FluentAssertions;
using HeimGuard;
using Microsoft.Extensions.Options;
using MockQueryable.Moq;
using Moq;
using Sieve.Models;
using Sieve.Services;
using TestHelpers;
using NUnit.Framework;

public class GetUserAccountListTests
{
    
    private readonly SieveProcessor _sieveProcessor;
    private readonly Mapper _mapper = UnitTestUtils.GetApiMapper();
    private readonly Mock<IUserAccountRepository> _userAccountRepository;
      private readonly Mock<IHeimGuardClient> _heimGuard;

    public GetUserAccountListTests()
    {
        _userAccountRepository = new Mock<IUserAccountRepository>();
        var sieveOptions = Options.Create(new SieveOptions());
        _sieveProcessor = new SieveProcessor(sieveOptions);
        _heimGuard = new Mock<IHeimGuardClient>();
    }
    
    [Test]
    public async Task can_get_paged_list_of_userAccount()
    {
        //Arrange
        var fakeUserAccountOne = FakeUserAccount.Generate();
        var fakeUserAccountTwo = FakeUserAccount.Generate();
        var fakeUserAccountThree = FakeUserAccount.Generate();
        var userAccount = new List<UserAccount>();
        userAccount.Add(fakeUserAccountOne);
        userAccount.Add(fakeUserAccountTwo);
        userAccount.Add(fakeUserAccountThree);
        var mockDbData = userAccount.AsQueryable().BuildMock();
        
        var queryParameters = new UserAccountParametersDto() { PageSize = 1, PageNumber = 2 };

        _userAccountRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);
        
        //Act
        var query = new GetUserAccountList.Query(queryParameters);
        var handler = new GetUserAccountList.Handler(_userAccountRepository.Object, _mapper, _sieveProcessor, _heimGuard.Object);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.Should().HaveCount(1);
    }




}