using ExpenseTrackingApp.Infrastructure.Repositories.ExpenseRepository;
using ExpenseTrackingApp.Infrastructure.Repositories.HelperModels;
using ExpenseTrackingApp.Services.Services.Schedule;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTrackingApp.Services.Services.ExpenseService
{
	public class ExpenseService: IExpenseService
	{

		private readonly IExpenseRepository _expenseRepository;
		private readonly IUserRepository _userRepository;
		
		public ExpenseService(IExpenseRepository expenseRepository, IUserRepository userRepository)
		{
			_expenseRepository = expenseRepository;
			_userRepository = userRepository;
		}

		public async Task CreateExpense(CreateExpenseRequest request)
		{
			var data = request.Adapt<Expense>();
			await _expenseRepository.CreateAsync(data);

		}
		public async Task<IList<DailyExpenseDto>> GetDailyExpensesAsync(int id)
		{
			var data = await _expenseRepository.GetDailyExpensesAsync(id);
			var dto = data.Adapt<IList<DailyExpenseDto>>();
			return dto;

		}

		public async Task<IList<WeeklyExpenseDto>> GetWeeklyExpensesAsync(int id)
		{
			var data = await _expenseRepository.GetWeeklyExpensesAsync(id);
			var dto = data.Adapt<IList<WeeklyExpenseDto>>();
			return dto;

		}

		public async Task<IList<MonthlyExpenseDto>> GetMonthlyExpensesAsync(int id)
		{
			var data = await _expenseRepository.GetMonthlyExpensesAsync(id);
			var dto = data.Adapt<IList<MonthlyExpenseDto>>();
			return dto;

		}

		public async Task SubscribeToDaily(int id, string email)
		{
			var dailyData = await GetDailyExpensesAsync(id);
			if (dailyData != null)
			{
				ScheduleService.ScheduleExpenseSubscription(dailyData, email);
			}
			

		}

	}
}
