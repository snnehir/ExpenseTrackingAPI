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

        public async Task<int> GetCurrentUserId()
        {
            var userId = int.Parse(_httpContextAccessor.HttpContext.User.Claims.First(i => i.Type == "UserId").Value);
            return userId;
        }

		public async Task<IList<IGrouping<DateTime, Expense>>> GetDaily(int id)
		{
            return await _userRepository.GetUserExpensesAsync(id);
		}
	}
}
