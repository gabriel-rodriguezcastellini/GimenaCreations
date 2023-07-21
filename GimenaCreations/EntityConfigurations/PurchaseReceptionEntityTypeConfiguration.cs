using GimenaCreations.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GimenaCreations.EntityConfigurations;

public class PurchaseReceptionEntityTypeConfiguration : IEntityTypeConfiguration<PurchaseReception>
{
    public void Configure(EntityTypeBuilder<PurchaseReception> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.InvoiceNumber).IsRequired();
        builder.Property(x => x.InputDate).IsRequired();
        builder.Property(x => x.InputNumber).IsRequired();
        builder.Property(x => x.InvoiceDate).IsRequired();
        builder.Property(x => x.InvoiceNumber).IsRequired();
        builder.Property(x => x.InvoiceReceptionDate).IsRequired();
    }
}
