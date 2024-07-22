using ExpenseTrackingApp.Infrastructure.Repositories.HelperModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTrackingApp.Infrastructure.Repositories.ExpenseRepository
{
	public class EFExpenseRepository : IExpenseRepository
	{
		private readonly ExpenseTrackingAppDbContext _dbContext;
		public EFExpenseRepository(ExpenseTrackingAppDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task CreateAsync(Expense entity)
		{
			await _dbContext.Expenses.AddAsync(entity);
			await _dbContext.SaveChangesAsync();
		}

		public async Task<IList<Expense>> GetAllAsync()
		{
			return await _dbContext.Expenses.AsNoTracking().ToListAsync();
		}

		public async Task<DailyExpenseData> GetCurrentDayExpenseAsync(int id)
		{
			var today = DateTime.Now.Date;
			var filteredExpenses = await _dbContext.Expenses.Where(n => n.UserId == id && n.Created.Date == today).ToListAsync();
			var expense = new DailyExpenseData
			{
				Expenses = filteredExpenses,
				Date = DateTime.Now
			};
			return expense;
		}

		public async Task<IList<DailyExpenseData>> GetDailyExpensesAsync(int id)
		{
			var groupedExpenses = await _dbContext.Expenses
				.Where(n => n.UserId == id)
				.GroupBy(d => new { d.Created.Year, d.Created.Month, d.Created.Day })
				.Select(g => new
				{
					Date = new DateTime(g.Key.Year, g.Key.Month, g.Key.Day),
					Expenses = g.ToList()
				})
				.ToListAsync();

			var expenses = groupedExpenses
				.Select(g => new DailyExpenseData
				{
					Expenses = g.Expenses,
					Date = g.Date
				})
				.ToList();

			return expenses;
		}


		public async Task<IList<MonthlyExpenseData>> GetMonthlyExpensesAsync(int id)
		{
			var groupedExpenses = await _dbContext.Expenses
				.Where(n => n.UserId == id)
				.GroupBy(d => new { d.Created.Year, d.Created.Month })
				.Select(g => new
				{
					Year = g.Key.Year,
					Month = g.Key.Month,
					Expenses = g.ToList()
				})
				.ToListAsync();

			var monthlyExpenses = groupedExpenses.Select(g =>
			{
				var date = new DateTime(g.Year, g.Month, 1); 
				var formattedDate = date.ToString("MMMM yyyy");

				return new MonthlyExpenseData
				{
					Expenses = g.Expenses,
					Month = formattedDate
				};
			}).ToList();

			return monthlyExpenses;
		}


		public async Task<IList<WeeklyExpenseData>> GetWeeklyExpensesAsync(int id)
		{
			var groupedExpenses = await _dbContext.Expenses
				.Where(n => n.UserId == id)
				.GroupBy(d => new { d.Created.Year, Week = (d.Created.DayOfYear / 7) })
				.Select(g => new
				{
					Year = g.Key.Year,
					Week = g.Key.Week,
					Expenses = g.ToList()
				})
				.ToListAsync();

			return groupedExpenses.Select(g =>
			{
				var weekStartDate = new DateTime(g.Year, 1, 1).AddDays(g.Week * 7);
				var weekEndDate = weekStartDate.AddDays(6);

				return new WeeklyExpenseData
				{
					Expenses = g.Expenses,
					Date = $"{weekStartDate:dd MMMM yyyy} - {weekEndDate:dd MMMM yyyy}"
				};
			}).ToList();
		}


		public async Task UpdateAsync(Expense entity)
		{
			_dbContext.Expenses.Update(entity);
			await _dbContext.SaveChangesAsync();
		}
	}
}
