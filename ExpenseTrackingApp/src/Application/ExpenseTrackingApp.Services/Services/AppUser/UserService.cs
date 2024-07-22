using Microsoft.AspNetCore.Http;

namespace ExpenseTrackingApp.Services.Services.AppUser
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserService(IUserRepository userRepository, IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> CheckUserExistById(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            return user != null;
        }

        public async Task CreateAsync(User user)
        {
            await _userRepository.CreateAsync(user);
        }

        public async Task<User?> GetUserByEmail(string email)
        {
            return await _userRepository.GetUserByEmailAsync(email);
        }

        public async Task<BaseResponseModel<int>> GetCurrentUserId()
        {
			var _userId = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
			if (_userId == null)
			{
				return BaseResponseModel<int>.Fail(ConstantMessages.UserNotFound);
			}
			var userId = int.Parse(_userId);
			return BaseResponseModel<int>.Success(userId);
		}

		public async Task<BaseResponseModel<UserExpensesResponse>> GetUserExpenses()
		{
			var userIdResponse = await GetCurrentUserId();
			if (userIdResponse.Succeeded)
			{
				var data = await _userRepository.GetUserTotalExpenses(userIdResponse.Data);
				var userExpenseDto = new UserExpensesResponse { TotalExpenses = data };
				return BaseResponseModel<UserExpensesResponse>.Success(userExpenseDto);

			}
			return BaseResponseModel<UserExpensesResponse>.Fail(ConstantMessages.UserNotFound);
			

		}
	}
}
