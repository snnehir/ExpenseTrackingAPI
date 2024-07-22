using ExpenseTrackingApp.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTrackingApp.Infrastructure.Data
{
	public class ExpenseTrackingAppDbContext: DbContext
	{
		public DbSet<User> Users { get; set; }
		public DbSet<Expense> Expenses { get; set; }

		public ExpenseTrackingAppDbContext(DbContextOptions<ExpenseTrackingAppDbContext> options) : base(options)
		{

		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{

			modelBuilder.Entity<Expense>().HasOne(e => e.User)
										   .WithMany(u => u.Expenses)
										   .HasForeignKey(e => e.UserId);

			modelBuilder.Entity<Expense>().Property(e => e.Amount).HasColumnType("decimal(10,2)");

			modelBuilder.Entity<Expense>().Property(x => x.Created).HasDefaultValueSql("getdate()");
			modelBuilder.Entity<User>().HasIndex(x => x.Email).IsUnique();
		}
	}
}
