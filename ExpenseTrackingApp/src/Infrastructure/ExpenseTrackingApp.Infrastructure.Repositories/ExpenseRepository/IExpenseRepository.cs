using ExpenseTrackingApp.Infrastructure.Repositories.HelperModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTrackingApp.Infrastructure.Repositories.ExpenseRepository
{
	public interface IExpenseRepository : IRepository<Expense>
	{
		Task<IList<DailyExpenseData>> GetDailyExpensesAsync(int id);

		Task<IList<WeeklyExpenseData>> GetWeeklyExpensesAsync(int id);

		Task<IList<MonthlyExpenseData>> GetMonthlyExpensesAsync(int id);


	}
}
