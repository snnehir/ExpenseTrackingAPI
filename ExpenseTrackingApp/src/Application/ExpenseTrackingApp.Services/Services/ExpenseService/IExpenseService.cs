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
		Task<BaseResponseModel<IList<DailyExpenseDto>>> GetDailyExpensesAsync();

		Task<BaseResponseModel<IList<WeeklyExpenseDto>>> GetWeeklyExpensesAsync();

		Task<BaseResponseModel<IList<MonthlyExpenseDto>>> GetMonthlyExpensesAsync();

		Task SubscribeToDaily();

		Task CreateExpense(CreateExpenseRequest request);
	}
}
