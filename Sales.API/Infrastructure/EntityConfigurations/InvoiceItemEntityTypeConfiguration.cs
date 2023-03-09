using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sales.API.Model;

namespace Sales.API.Infrastructure.EntityConfigurations;
class InvoiceItemEntityTypeConfiguration
    : IEntityTypeConfiguration<InvoiceItem>
{
    public void Configure(EntityTypeBuilder<InvoiceItem> builder)
    {
        builder.ToTable("InvoiceItem");

        builder.HasKey(ci => ci.Id);

        builder.Property(cb => cb.ProductCode)
            .IsRequired()
            .HasMaxLength(100);

        //TODO: add other applicable db constraints here
    }
}

