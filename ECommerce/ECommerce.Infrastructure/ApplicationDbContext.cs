using MediatR;
using Microsoft.EntityFrameworkCore;
using ECommerce.Domain.Models;

namespace ECommerce.Infrastructure;

public class ApplicationDbContext : DbContext
{
    private readonly IMediator? _mediator;

    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        IMediator? mediator = null)
        : base(options)
    {
        _mediator = mediator;
    }

    public virtual DbSet<Product> Products { get; set; } = null!;

    public virtual DbSet<Price> Prices { get; set; } = null!;

    public virtual DbSet<Stock> Stocks { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>(eb =>
        {
            eb.HasKey(p => p.Id);
            eb.HasIndex(p => p.Sku).IsUnique();
            eb.Property(p => p.Sku).IsRequired();
            eb.Property(p => p.Name).IsRequired();

            eb.HasData(
                new Product { Id = 1, Sku = "SKU1", Name = "Sample Product 1", ImageUrl = "https://via.placeholder.com/150" },
                new Product { Id = 2, Sku = "SKU2", Name = "Sample Product 2", ImageUrl = "https://via.placeholder.com/150" }
            );
        });

        modelBuilder.Entity<Price>(eb =>
        {
            eb.HasKey(p => p.Id);
            eb.Property(p => p.ProductSku).IsRequired();
            eb.Property(p => p.Amount);
            eb.Property(p => p.Currency).HasMaxLength(8);
        });

        modelBuilder.Entity<Stock>(eb =>
        {
            eb.HasKey(s => s.Id);
            eb.Property(s => s.ProductSku).IsRequired();
        });
    }
}
