using GimenaCreations.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GimenaCreations.EntityConfigurations;

public class SupplierEntityTypeConfiguration : IEntityTypeConfiguration<Supplier>
{
    public void Configure(EntityTypeBuilder<Supplier> builder)
    {
        builder.ToTable("Suppliers");
        builder.HasKey(t => t.Id);
        builder.HasIndex(t => t.SupplierId).IsUnique();
        builder.Property(x => x.SupplierId).IsRequired();
        builder.Property(x => x.SupplierType).IsRequired();
        builder.Property(x => x.Phone1).IsRequired();
        builder.Property(x => x.Phone2).IsRequired();
        builder.Property(x => x.ContactName).IsRequired();
        builder.Property(x => x.BusinessName).IsRequired();
        builder.Property(x => x.Address).IsRequired();
        builder.Property(x => x.City).IsRequired();
        builder.Property(x => x.State).IsRequired();
        builder.Property(x => x.Email).IsRequired();
        builder.Property(x => x.Image).IsRequired(false);
    }
}
