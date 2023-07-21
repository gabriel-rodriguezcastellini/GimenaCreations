using GimenaCreations.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GimenaCreations.EntityConfigurations;

public class OrderItemEntityTypeConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable("OrderItems");
        builder.HasKey(o => o.Id);
        builder.Property(o => o.Id).UseHiLo("orderitemseq");                
        builder.Property(o => o.ProductName).IsRequired();
        builder.Property(o => o.PictureUrl).IsRequired();
        builder.Property(o => o.UnitPrice).IsRequired();
        builder.Property(o => o.Units).IsRequired();
    }
}
