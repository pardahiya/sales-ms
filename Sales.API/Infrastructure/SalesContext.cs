using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Sales.API.Infrastructure.EntityConfigurations;
using Sales.API.Model;

namespace Sales.API.Infrastructure;
public class SalesContext : DbContext
{
    public SalesContext(DbContextOptions<SalesContext> options) : base(options)
    {
    }
    public DbSet<Invoice> Invoices { get; set; }
    public DbSet<InvoiceItem> InvoiceItems { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new InvoiceEntityTypeConfiguration());
        builder.ApplyConfiguration(new InvoiceItemEntityTypeConfiguration());
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=127.0.0.1,5433;Initial Catalog=SalesDb;TrustServerCertificate=True;Encrypt=False;MultiSubnetFailover=True;User Id=sa;Password=Pass@w0rd");
    }
}


public class SalesContextDesignFactory : IDesignTimeDbContextFactory<SalesContext>
{
    public SalesContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<SalesContext>()
            .UseSqlServer("Server=.;Initial Catalog=SalesDb;Integrated Security=true");

        return new SalesContext(optionsBuilder.Options);
    }
}