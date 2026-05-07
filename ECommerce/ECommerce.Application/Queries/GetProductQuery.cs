using MediatR;

namespace ECommerce.Application.Queries;

public record GetProductQuery(string Sku) : IRequest<Abstraction.ProductDto?>;

