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
		public async Task<IActionResult> Daily()
		{
			var result = await _expenseService.GetDailyExpensesAsync();

			return Ok(result);

		}

		[HttpGet("weekly")]		
		public async Task<IActionResult> Weekly( )
		{
			var result = await _expenseService.GetWeeklyExpensesAsync();

			return Ok(result);

		}

		[HttpGet("monthly")]
		public async Task<IActionResult> Monthly( )
		{
			var result = await _expenseService.GetMonthlyExpensesAsync();

			return Ok(result);

		}

		[HttpGet("subscription/daily")]
		public async Task<IActionResult> Subscription( )
		{
			await _expenseService.SubscribeToDaily();

			return Ok();

		}

		// Dummy controller for testing transaction => Delete expense and decrease user's expense count
		[HttpDelete("{expenseId}")]
		public async Task<IActionResult> Delete([FromRoute] int expenseId)
		{
			var result = await _expenseService.DeleteExpense(expenseId);

			return Ok(result);

		}
	}
}
