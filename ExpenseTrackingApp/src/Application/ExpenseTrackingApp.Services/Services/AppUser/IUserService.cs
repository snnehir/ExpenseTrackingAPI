namespace ExpenseTrackingApp.Services.Services.AppUser
{
    public interface IUserService
    {
        Task<bool> CheckUserExistById(int id);
        Task<User?> GetUserByEmail(string email);
        Task CreateAsync(User user);
        Task<BaseResponseModel<int>> GetCurrentUserId();

        Task<BaseResponseModel<UserExpensesResponse>> GetUserExpenses();
		Task<BaseResponseModel<bool>> DecreaseUserExpenseCount(int userId);

	}
}
