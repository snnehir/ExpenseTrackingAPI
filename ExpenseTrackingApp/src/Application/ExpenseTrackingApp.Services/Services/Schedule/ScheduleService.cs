using ExpenseTrackingApp.Services.Services.Mail;
using Hangfire;

namespace ExpenseTrackingApp.Services.Services.Schedule;

public class ScheduleService
{
	public static void ScheduleSendRegisterEmailWithPassword(string name, string lastName, string email, string password)
	{
		BackgroundJob.Schedule<IEmailService>(x => x.SendRegisterEmailWithPassword(name, lastName, email, password), TimeSpan.FromMinutes(1));
	}
	public static void ScheduleExpenseSubscription(IList<DailyExpenseDto> expenses, string email)
	{
		BackgroundJob.Schedule<IEmailService>(x => x.SendDailyExpenseMail(expenses, email), TimeSpan.FromMinutes(1));
	}
}