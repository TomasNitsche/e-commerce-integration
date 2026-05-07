using MediatR;
using ECommerce.Application.Queries;
using ECommerce.Application.Abstraction;

namespace ECommerce.Application.Handlers;

public class GetProductQueryHandler(IProductService productService) : IRequestHandler<GetProductQuery, ProductDto?>
{
    public async Task<ProductDto?> Handle(GetProductQuery request, CancellationToken cancellationToken)
    {
        return await productService.GetProductAsync(request.Sku, cancellationToken);
    }
}

