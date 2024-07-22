using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTrackingApp.DataTransferObjects.Responses
{
	public class UserExpense
	{
		public int Id { get; set; }
		public decimal Amount { get; set; }
		public string Description { get; set; }
		public DateTime Created { get; set; }
	}
}
