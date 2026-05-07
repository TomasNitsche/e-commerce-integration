using MediatR;

namespace ECommerce.Application.Commands;

public record AddProductCommand(string Sku, string Name, string? ImageUrl) : IRequest;

