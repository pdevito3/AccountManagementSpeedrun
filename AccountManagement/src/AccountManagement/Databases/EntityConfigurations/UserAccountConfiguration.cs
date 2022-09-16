namespace AccountManagement.Databases.EntityConfigurations;

using AccountManagement.Domain.UserAccounts;
using Domain.MonetaryAmounts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public sealed class UserAccountConfiguration : IEntityTypeConfiguration<UserAccount>
{
    /// <summary>
    /// The database configuration for UserAccounts. 
    /// </summary>
    public void Configure(EntityTypeBuilder<UserAccount> builder)
    {
        builder.Property(x => x.Balance)
            .HasConversion(x => x.Amount, x => new MonetaryAmount(x))
            .HasColumnName("balance");
    }
}