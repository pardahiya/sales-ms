using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sales.API.Model;

namespace Sales.API.Infrastructure.EntityConfigurations;
class InvoiceEntityTypeConfiguration
    : IEntityTypeConfiguration<Invoice>
{
    public void Configure(EntityTypeBuilder<Invoice> builder)
    {
        builder.ToTable("Invoice");

        builder.HasKey(ci => ci.Id);

        builder.Property(cb => cb.InvoiceNumber)
            .IsRequired();

        //TODO: add more db constraints here
    }
}