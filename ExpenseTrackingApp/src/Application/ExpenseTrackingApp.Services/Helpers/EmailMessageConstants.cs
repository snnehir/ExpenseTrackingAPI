using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTrackingApp.Services.Helpers
{
	public static class EmailMessageConstants
	{
		public const string RegisterTitle = "You are successfuly registered Expense Tracking App";
		public const string RegisterSubject = "You are successfuly registered Expense Tracking App";

		public static string GetRegisterBody(string name)
		{
			return $"<p>Hello {name}</p><br><p>Welcome to the ExpenseTrackingApp App</p>";
		}

		public static string GetRegisterBodyWithPassword(string name, string lastName)
		{
			return $"<p>Hello {name} {lastName}</p><br><p>Welcome to the ExpenseTrackingApp App</p><br>";
		}

		public static string GetDailyMailBody(string text)
		{
			return $"<p>Hello! Your daily expenses mailed!</p> </br> {text}";
		}

	}
}
