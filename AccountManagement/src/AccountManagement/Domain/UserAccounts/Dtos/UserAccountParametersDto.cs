namespace AccountManagement.Domain.UserAccounts.Dtos;

using SharedKernel.Dtos;

public sealed class UserAccountParametersDto : BasePaginationParameters
{
    public string Filters { get; set; }
    public string SortOrder { get; set; }
}
