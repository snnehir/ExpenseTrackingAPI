using Serilog;

namespace ExpenseTrackingApp.WebAPI.Middlewares
{
	public class ExceptionHandlingMiddleware
	{
		private readonly RequestDelegate _next;

		public ExceptionHandlingMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			try
			{
				await _next(context);
			}
			catch (Exception exception)
			{
				string errorMessage = $"Exception message:  {exception.Message}";
				Log.Error(exception, errorMessage);

				context.Response.StatusCode = StatusCodes.Status500InternalServerError;
				await context.Response.WriteAsJsonAsync(new
				{
					Title = "Server Error",
					Status = context.Response.StatusCode,
					Message = errorMessage
				});
			}
		}
	}
}
