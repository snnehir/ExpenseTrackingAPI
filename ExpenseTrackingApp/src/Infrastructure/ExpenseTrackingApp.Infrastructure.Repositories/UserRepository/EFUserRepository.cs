using ExpenseTrackingApp.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTrackingApp.Infrastructure.Repositories.UserRepository
{
    public class EFUserRepository : IUserRepository
    {
        private readonly ExpenseTrackingAppDbContext _dbContext;
        public EFUserRepository(ExpenseTrackingAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task CreateAsync(User entity)
        {
            await _dbContext.Users.AddAsync(entity);
			await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var user = await _dbContext.Users.FindAsync(id);
			if(user == null)
			{
				throw new Exception("User not found!");
			}
			await DeleteAsync(user);
        }

		public async Task DeleteAsync(User entity)
		{
			_dbContext.Users.Remove(entity);
			await _dbContext.SaveChangesAsync();
		}

		public async Task<IList<User>> GetAllAsync() => await _dbContext.Users.AsNoTracking().ToListAsync();

        public async Task<User?> GetByIdAsync(int id) => await _dbContext.Users.SingleOrDefaultAsync(u => u.Id == id);

		public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _dbContext.Users.SingleOrDefaultAsync(u => u.Email.Equals(email));
        }

		public async Task<IList<Expense>> GetUserExpensesAsync(int id)
		{
			return await _dbContext.Expenses.Where(e => e.UserId == id).ToListAsync();
		}

		public async Task<IEnumerable<Expense>> GetUserExpensesDaily(int id)
		{

			var groupedExpenses = await _dbContext.Expenses
		.Where(n => n.UserId == id) // Assuming you're grouping by UserId, not by Expense Id
		.GroupBy(d => new { d.Created.Year, d.Created.Month, d.Created.Day })
		.Select(g => new
		{
			Year = g.Key.Year,
			Month = g.Key.Month,
			Day = g.Key.Day,
			Expenses = g.ToList()
		})
		.ToListAsync();

			var expenses = groupedExpenses.SelectMany(g => g.Expenses).ToList();
			return expenses;
		}

		public async Task<decimal> GetUserTotalExpenses(int id)
		{
			var expenses = await _dbContext.Expenses.Where(e => e.UserId == id).ToListAsync();
			var totalAmount = expenses.Sum(e => e.Amount);

			return  totalAmount;
		}

		/*public async Task<IList<Expense>> GetUserDailyExpensesAsync(int id)
		{
			var x = await _dbContext.Expenses.Where(n => n.Id == id).ToListAsync();
			
            return x;
		}*/

		/*public async Task<IList<Expense>> GetUserDailyExpensesAsync(int id)
		{
			// return await _dbContext.Expenses.Where(n => n.Id == id).ToListAsync();
			var x = await _dbContext.Expenses.Where(n => n.Id == id).GroupBy(d => new { d.Created.Year, d.Created.Month, d.Created.Day }).SelectMany(a => a).ToListAsync();
			return x;
		}*/


		// throws exception randomly for testing transaction example
		public async Task UpdateAsync(User entity)
        {
			Random random = new Random();
			int randNum = random.Next(1, 3);

			if(randNum == 1)
			{
				throw new Exception("User update exception!");
			}

			_dbContext.Users.Update(entity);
            await _dbContext.SaveChangesAsync();
        }
    }
}
