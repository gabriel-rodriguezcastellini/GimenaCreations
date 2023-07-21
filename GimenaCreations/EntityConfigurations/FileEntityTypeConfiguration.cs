using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using File = GimenaCreations.Entities.File;

namespace GimenaCreations.EntityConfigurations;

public class FileEntityTypeConfiguration : IEntityTypeConfiguration<File>
{
    public void Configure(EntityTypeBuilder<File> builder)
    {
        builder.ToTable("Files");
        builder.Property(x=>x.Name).IsRequired();
    }
}
