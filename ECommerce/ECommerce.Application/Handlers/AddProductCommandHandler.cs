using MediatR;
using ECommerce.Application.Commands;
using ECommerce.Application.Abstraction;

namespace ECommerce.Application.Handlers;

public class AddProductCommandHandler(IProductService productService) : IRequestHandler<AddProductCommand>
{
    public async Task<Unit> Handle(AddProductCommand request, CancellationToken cancellationToken)
    {
        await productService.AddProductAsync(request.Sku, request.Name, request.ImageUrl, cancellationToken);
        return Unit.Value;
    }
}

