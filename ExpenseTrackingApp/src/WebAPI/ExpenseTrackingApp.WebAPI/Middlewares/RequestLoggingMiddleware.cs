using Microsoft.AspNetCore.Http;
using Serilog;
using System.Threading.Tasks;


namespace ExpenseTrackingApp.WebAPI.Middlewares
{
	public class RequestLoggingMiddleware
	{
		private readonly RequestDelegate _next;

		public RequestLoggingMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public async Task InvokeAsync(HttpContext context)
		{
            Console.WriteLine("Burası.............");
            Log.Information("{Method} {Path}", context.Request.Method, context.Request.Path);

			await _next(context);

		}
	}

}
