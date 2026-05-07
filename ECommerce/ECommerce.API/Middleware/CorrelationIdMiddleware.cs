using System.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace ECommerce.API.Middleware;

public class CorrelationIdMiddleware
{
	private readonly RequestDelegate _next;
	public const string HeaderKey = "X-Correlation-Id";

	public CorrelationIdMiddleware(RequestDelegate next)
	{
		_next = next;
	}

	public async Task InvokeAsync(HttpContext context)
	{
		if (!context.Request.Headers.TryGetValue(HeaderKey, out var cid) || string.IsNullOrWhiteSpace(cid))
		{
			cid = Activity.Current?.Id ?? Guid.NewGuid().ToString();
			context.Request.Headers[HeaderKey] = cid;
		}

		context.Response.OnStarting(() => {
			// use indexer to avoid ArgumentException when header already present
			context.Response.Headers[HeaderKey] = cid.ToString();
			return Task.CompletedTask;
		});

		await _next(context);
	}
}


