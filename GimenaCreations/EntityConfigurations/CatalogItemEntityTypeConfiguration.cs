using GimenaCreations.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GimenaCreations.EntityConfigurations;

class CatalogItemEntityTypeConfiguration : IEntityTypeConfiguration<CatalogItem>
{
    public void Configure(EntityTypeBuilder<CatalogItem> builder)
    {
        builder.ToTable("Catalog");
        builder.Property(ci => ci.Id).UseHiLo("catalog_hilo").IsRequired();
        builder.Property(ci => ci.Name).IsRequired().HasMaxLength(50);
        builder.Property(ci => ci.Price).IsRequired();
        builder.Property(ci => ci.PictureFileName).IsRequired(false);
        builder.HasOne(ci => ci.CatalogType).WithMany().HasForeignKey(ci => ci.CatalogTypeId);
    }
}
