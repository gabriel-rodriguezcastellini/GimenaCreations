using GimenaCreations.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GimenaCreations.EntityConfigurations;

public class OrderEntityTypeConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Orders");
        builder.HasKey(o => o.Id);
        builder.Property(o => o.Id).UseHiLo("orderseq");
        builder.Property(x=>x.ApplicationUserId).IsRequired();

        builder.OwnsOne(x => x.Address, x =>
        {
            x.WithOwner();
            x.Property(x => x.Street).IsRequired();
            x.Property(x => x.ZipCode).IsRequired();
            x.Property(x => x.City).IsRequired();
            x.Property(x => x.State).IsRequired();
            x.Property(x => x.Country).IsRequired();
        });

        builder.Navigation(x => x.Address).IsRequired();
    }
}
