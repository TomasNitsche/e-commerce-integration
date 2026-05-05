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
}
