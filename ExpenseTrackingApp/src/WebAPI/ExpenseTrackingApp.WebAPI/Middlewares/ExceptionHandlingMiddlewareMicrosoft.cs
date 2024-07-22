namespace ExpenseTrackingApp.WebAPI.Middlewares
{
	public class ExceptionHandlingMiddlewareMicrosoft(ILogger<ExceptionHandlingMiddlewareMicrosoft> logger, RequestDelegate next)
	{
		// works in every http request
		public async Task InvokeAsync(HttpContext context)
		{
			try
			{
				await next(context);
			}
			catch (Exception exception)
			{
				string errorMessage = $"Exception message:  {exception.Message}";
				logger.LogError(exception, errorMessage);

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
