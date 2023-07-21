using GimenaCreations.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GimenaCreations.EntityConfigurations;

public class PurchaseHistoryItemEntityTypeConfiguration : IEntityTypeConfiguration<PurchaseHistoryItem>
{
    public void Configure(EntityTypeBuilder<PurchaseHistoryItem> builder)
    {
        builder.Property(x=>x.State).IsRequired();
        builder.Property(x => x.Date).IsRequired();
    }
}
