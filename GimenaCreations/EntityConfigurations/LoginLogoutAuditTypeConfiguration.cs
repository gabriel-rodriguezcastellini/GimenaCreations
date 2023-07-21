using GimenaCreations.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GimenaCreations.EntityConfigurations;

public class LoginLogoutAuditTypeConfiguration : IEntityTypeConfiguration<LoginLogoutAudit>
{
    public void Configure(EntityTypeBuilder<LoginLogoutAudit> builder)
    {
        builder.Property(x=>x.ApplicationUserId).IsRequired();
        builder.Property(x => x.Username).IsRequired();
        builder.Property(x => x.FullName).IsRequired();        
    }
}
