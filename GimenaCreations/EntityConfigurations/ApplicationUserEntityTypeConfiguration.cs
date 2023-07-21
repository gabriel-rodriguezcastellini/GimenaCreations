using GimenaCreations.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GimenaCreations.EntityConfigurations;

public class ApplicationUserEntityTypeConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
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
        builder.Property(x => x.FirstName).IsRequired();
        builder.Property(x => x.LastName).IsRequired();        
    }
}
