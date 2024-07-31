using ExpenseTrackingApp.Entities;
using ExpenseTrackingApp.Infrastructure.Repositories.ExpenseRepository;
using ExpenseTrackingApp.Infrastructure.Repositories.HelperModels;
using ExpenseTrackingApp.Services.Services.Schedule;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace ExpenseTrackingApp.Services.Services.ExpenseService
{
	public class ExpenseService: IExpenseService
	{

		private readonly IExpenseRepository _expenseRepository;
		private readonly IHttpContextAccessor _accessor;
		private readonly IUserService _userService;

		public ExpenseService(IExpenseRepository expenseRepository, IHttpContextAccessor accessor, IUserService userService)
		{
			_expenseRepository = expenseRepository;
			_accessor = accessor;
			_userService = userService;
		}

		public async Task CreateExpense(CreateExpenseRequest request)
		{
			var data = request.Adapt<Expense>();
			await _expenseRepository.CreateAsync(data);

		}
		public async Task<BaseResponseModel<IList<DailyExpenseDto>>> GetDailyExpensesAsync()
		{
			var userIdResponse = await _userService.GetCurrentUserId();
			if (userIdResponse.Succeeded)
			{
				var data = await _expenseRepository.GetDailyExpensesAsync(userIdResponse.Data);
				var dto = data.Adapt<IList<DailyExpenseDto>>();
				return BaseResponseModel<IList<DailyExpenseDto>>.Success(dto);

			}
			return BaseResponseModel<IList<DailyExpenseDto>>.Fail(ConstantMessages.UserNotFound);

		}

		public async Task<BaseResponseModel<IList<WeeklyExpenseDto>>> GetWeeklyExpensesAsync()
		{
			var userIdResponse = await _userService.GetCurrentUserId();
			if (userIdResponse.Succeeded)
			{
				var data = await _expenseRepository.GetWeeklyExpensesAsync(userIdResponse.Data);
				var dto = data.Adapt<IList<WeeklyExpenseDto>>();
				return BaseResponseModel<IList<WeeklyExpenseDto>>.Success(dto);

			}
			return BaseResponseModel<IList<WeeklyExpenseDto>>.Fail(ConstantMessages.UserNotFound);

		}

		public async Task<BaseResponseModel<IList<MonthlyExpenseDto>>> GetMonthlyExpensesAsync()
		{
			var userIdResponse = await _userService.GetCurrentUserId();
			if (userIdResponse.Succeeded)
			{
				var data = await _expenseRepository.GetMonthlyExpensesAsync(userIdResponse.Data);
				var dto = data.Adapt<IList<MonthlyExpenseDto>>();
				return BaseResponseModel<IList<MonthlyExpenseDto>>.Success(dto);

			}
			return BaseResponseModel<IList<MonthlyExpenseDto>>.Fail(ConstantMessages.UserNotFound);
		}

		// Scheduling services
		public async Task<BaseResponseModel<DailyExpenseDto>> GetCurrentDayExpenseAsync()
		{
			var userIdResponse = await _userService.GetCurrentUserId();
			if (userIdResponse.Succeeded)
			{
				var data = await _expenseRepository.GetCurrentDayExpenseAsync(userIdResponse.Data);
				var dto = data.Adapt<DailyExpenseDto>();
				return BaseResponseModel<DailyExpenseDto>.Success(dto);
			}

			return BaseResponseModel<DailyExpenseDto>.Fail(ConstantMessages.UserNotFound);
		}
		public async Task SubscribeToDaily()
		{
			var _userId = _accessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
			var email = _accessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
			if (_userId != null && email != null)
			{
				int userId = int.Parse(_userId);
				var dailyData = await GetCurrentDayExpenseAsync();
				if (dailyData != null)
				{
					ScheduleService.ScheduleExpenseSubscription(dailyData.Data, email);
				}
			}

		}


		// Dummy service action for testing transaction => Delete expense and decrease user's expense count
		public async Task<BaseResponseModel<bool>> DeleteExpense(int expenseId)
		{
			var _userId = _accessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
			var email = _accessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
			
			if (_userId != null && email != null)
			{
				try
				{
					using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
					{
						int userId = int.Parse(_userId);

						// transaction 1
						await _expenseRepository.DeleteAsync(expenseId, userId);

						// transaction 2 (if this action cannot be completed, transaction 1 will be cancelled.)
						await _userService.DecreaseUserExpenseCount(userId);

						scope.Complete();
						return BaseResponseModel<bool>.Success();	
					}
				}
				catch (Exception e)
				{
					return BaseResponseModel<bool>.Fail("Delete expense failed with exception: " + e.Message);
				}
				

			}

			return BaseResponseModel<bool>.Fail(ConstantMessages.UserNotFound);
		}

	}
}
