namespace ExpenseTrackingApp.Services.Services.AppUser
{
    public interface IUserService
    {
        Task<bool> CheckUserExistById(int id);
        Task<User?> GetUserByEmail(string email);
        Task CreateAsync(User user);
        Task<int> GetCurrentUserId();

		Task<IList<IGrouping<DateTime, Expense>>> GetDaily(int id);
    }
}
