using GimenaCreations.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GimenaCreations.EntityConfigurations;

public class PurchaseEntityTypeConfiguration : IEntityTypeConfiguration<Purchase>
{
    public void Configure(EntityTypeBuilder<Purchase> builder)
    {
        builder.ToTable("Purchases");
        builder.HasKey(p => p.Id);
        builder.Property(x => x.RecipientName).IsRequired();
        builder.Property(x => x.ShippingAddress).IsRequired();
        builder.Property(x => x.ShippingCity).IsRequired();
    }
}
