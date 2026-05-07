using ECommerce.API.DTO;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using ECommerce.Application.Queries;
using ECommerce.Application.Commands;

namespace ECommerce.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController(IMediator mediator) : ControllerBase
{
	[HttpGet("{sku}")]
	public async Task<IActionResult> Get(string sku)
	{
		var result = await mediator.Send(new GetProductQuery(sku));
		if (result is null) return NotFound();

		var apiDto = Mapping.ProductMapping.MapToDto(result);

		return Ok(apiDto);
	}

	[HttpPost]
	public async Task<IActionResult> Create([FromBody] AddProductRequest? request)
	{
		if (request is null) return BadRequest();

		await mediator.Send(new AddProductCommand(request.Sku, request.Name, request.ImageUrl));

		return CreatedAtAction(nameof(Get), new { sku = request.Sku }, null);
	}
}


