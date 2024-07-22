using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTrackingApp.DataTransferObjects.Responses
{
	public class DailyExpenseDto
	{
		public IList<UserExpense> Expenses { get; set; }
		public DateTime Date { get; set; }
	}
}
