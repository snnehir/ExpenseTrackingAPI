using System;

namespace ExpenseTrackingApp.Entities
{
	public class Expense: IEntity
	{
        public int Id { get; set; }
        public int UserId { get; set; }
		public decimal Amount { get; set; }
		public string Description { get; set; }
		public DateTime Created { get; set; }

		public User User { get; set; }
	}
}
