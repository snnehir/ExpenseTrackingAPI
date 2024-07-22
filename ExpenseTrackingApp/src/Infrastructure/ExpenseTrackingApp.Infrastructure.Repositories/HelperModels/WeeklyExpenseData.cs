using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTrackingApp.Infrastructure.Repositories.HelperModels
{
	public class WeeklyExpenseData
	{
		public IList<Expense> Expenses { get; set; }
		public string Date { get; set; }
	}
}
