using GimenaCreations.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GimenaCreations.EntityConfigurations
{
    public class AuditEntryEntityTypeConfiguration : IEntityTypeConfiguration<AuditEntry>
    {
        public void Configure(EntityTypeBuilder<AuditEntry> builder) 
        {
            builder.HasOne(x => x.User).WithMany().HasForeignKey(x => x.ApplicationUserId).IsRequired(false);
        }
    }
}
