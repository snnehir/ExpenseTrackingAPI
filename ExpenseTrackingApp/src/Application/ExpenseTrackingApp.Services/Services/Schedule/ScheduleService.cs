using ExpenseTrackingApp.Services.Services.Mail;
using Hangfire;

namespace ExpenseTrackingApp.Services.Services.Schedule;

public class ScheduleService
{
	public static void ScheduleSendRegisterEmailWithPassword(string name, string lastName, string email, string password)
	{
		BackgroundJob.Schedule<IEmailService>(x => x.SendRegisterEmailWithPassword(name, lastName, email, password), TimeSpan.FromMinutes(1));
	}
	// TODO: create job id and save in other table to make unsubscripe operations...
	public static void ScheduleExpenseSubscription(DailyExpenseDto expenses, string email)
	{
		RecurringJob.AddOrUpdate<IEmailService>("job-id-1", x => x.SendDailyExpenseMail(expenses, email), Cron.Daily);
	}
}