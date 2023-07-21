using GimenaCreations.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GimenaCreations.EntityConfigurations;

public class PurchaseReceptionItemEntityTypeConfiguration : IEntityTypeConfiguration<PurchaseReceptionItem>
{
    public void Configure(EntityTypeBuilder<PurchaseReceptionItem> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x=>x.ProductType).IsRequired();
        builder.Property(x => x.ProductName).IsRequired();
    }
}
