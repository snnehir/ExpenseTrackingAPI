namespace ExpenseTrackingApp.Infrastructure.Repositories.UserRepository
{
    public interface IUserRepository: IRepository<User>
    {
        Task<User?> GetByIdAsync(int id);
        Task DeleteAsync(int id);
        // get expenses
        Task<User?> GetUserByEmailAsync(string email);

		//Task<IList<Expense>> GetUserExpensesAsync(int id);
		Task<IList<IGrouping<DateTime, Expense>>> GetUserExpensesAsync(int id);
	}
}
