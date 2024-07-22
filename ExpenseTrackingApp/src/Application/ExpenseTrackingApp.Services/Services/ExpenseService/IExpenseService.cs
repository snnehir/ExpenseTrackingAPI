using ExpenseTrackingApp.Infrastructure.Repositories.HelperModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTrackingApp.Services.Services.ExpenseService
{
	public interface IExpenseService
	{
		public Task<IList<DailyExpenseDto>> GetDailyExpensesAsync(int id);

		Task<IList<WeeklyExpenseDto>> GetWeeklyExpensesAsync(int id);

		Task<IList<MonthlyExpenseDto>> GetMonthlyExpensesAsync(int id);

		Task SubscribeToDaily(int id, string email);

		Task CreateExpense(CreateExpenseRequest request);
	}
}
