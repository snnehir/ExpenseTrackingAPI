using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Http;
using MimeKit;
using ExpenseTrackingApp.Services.Helpers;
using System.Net.Mail;
using ExpenseTrackingApp.DataTransferObjects.Responses;
using Hangfire;
using System.Xml.Linq;
using System.Text;


namespace ExpenseTrackingApp.Services.Services.Mail
{
	public class EmailService : IEmailService
	{

		public EmailService()
		{
		}

		// [AutomaticRetry(Attempts = 0)]
		public async Task<BaseResponseModel<EmailResponse>> SendRegisterEmailWithPassword(string name, string lastName, string email,
		string password)
		{

            await SendEmail(new EmailMessage(new[] { email }, EmailMessageConstants.RegisterTitle, EmailMessageConstants.RegisterSubject,
				EmailMessageConstants.GetRegisterBodyWithPassword(name, lastName)));


			return BaseResponseModel<EmailResponse>.Success();

		}
		public async Task<BaseResponseModel<EmailResponse>> SendDailyExpenseMail(DailyExpenseDto dailyExpense, string email)
		{
			var sb = new StringBuilder();

			sb.AppendLine($"<p><strong> Day: {dailyExpense.Day}</strong></p>");
			sb.AppendLine("<p>Expenses:</p> <ul>");

			foreach (var expense in dailyExpense.Expenses)
			{
				sb.AppendLine($"<li> Amount: {expense.Amount:C}, Description: {expense.Description} </li>");
			}

			sb.AppendLine("</ul> </br>");
			sb.AppendLine("");
			var text = sb.ToString();
			await SendEmail(new EmailMessage(new[] { email }, EmailMessageConstants.DailyExpensesTitle, EmailMessageConstants.DailyExpensesSubject,
				EmailMessageConstants.GetDailyMailBody(text)));


			return BaseResponseModel<EmailResponse>.Success();
		}

		
		private async Task SendEmail(EmailMessage message)
		{
			var emailMessage = CreateEmailMessage(message);



			var client = new MailKit.Net.Smtp.SmtpClient();
				try
				{
					client.Connect(StaticDefinitions.Host, 587, false);
					client.Authenticate(StaticDefinitions.MailSenderAddress, StaticDefinitions.MailSenderPassword);
					client.Send(emailMessage);
					client.Disconnect(true);
				}
				finally
				{
					await client.DisconnectAsync(true).ConfigureAwait(false);
					client.Dispose();
				}
			
			

		}

		private MimeMessage CreateEmailMessage(EmailMessage message)
		{
			var emailMessage = new MimeMessage();

			emailMessage.From.Add(new MailboxAddress(message.Title, StaticDefinitions.MailSenderAddress));
			emailMessage.To.AddRange(message.To);
			emailMessage.Subject = message.Subject;
			var builder = new BodyBuilder();
			builder.HtmlBody = message.Content;
			emailMessage.Body = builder.ToMessageBody();
			return emailMessage;
		}

		
	}

}
