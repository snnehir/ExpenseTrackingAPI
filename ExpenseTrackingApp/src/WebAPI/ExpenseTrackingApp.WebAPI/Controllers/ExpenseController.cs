using ExpenseTrackingApp.Services.Services.ExpenseService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTrackingApp.WebAPI.Controllers
{
	[Route("api/[controller]")]
	[Authorize]
	public class ExpenseController : Controller
	{
		private readonly IExpenseService _expenseService;
		public ExpenseController(IExpenseService expenseService)
		{
			_expenseService = expenseService;
		}

		[HttpPost]
		public async Task<IActionResult> Create([FromBody] CreateExpenseRequest request)
		{
			await _expenseService.CreateExpense(request);

			return Ok();

		}


		[HttpGet("daily")]
		public async Task<IActionResult> Daily([FromQuery] int userId)
		{

            var result = await _expenseService.GetDailyExpensesAsync(userId);

			return Ok(result);

		}

		[HttpGet("weekly")]		
		public async Task<IActionResult> Weekly([FromQuery] int userId)
		{
			var result = await _expenseService.GetWeeklyExpensesAsync(userId);

			return Ok(result);

		}

		[HttpGet("monthly")]
		public async Task<IActionResult> Monthly([FromQuery] int userId)
		{
			var result = await _expenseService.GetMonthlyExpensesAsync(userId);

			return Ok(result);

		}

		[HttpGet("subscription/daily")]
		public async Task<IActionResult> Subscription([FromQuery] int userId)
		{
			// Burası nedense çalışmıyor
			await _expenseService.SubscribeToDaily(userId, "snnehir21@gmail.com");

			return Ok();

		}
	}
}
