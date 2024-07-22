using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTrackingApp.Infrastructure.Repositories.HelperModels
{
	public class DailyExpenseData
	{
		public IList<Expense> Expenses { get; set; }
		public DateTime Date { get; set; }
	}
}
