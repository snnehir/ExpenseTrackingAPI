using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Http;
using MimeKit;
using ExpenseTrackingApp.Services.Helpers;
using System.Net.Mail;
using ExpenseTrackingApp.DataTransferObjects.Responses;
using Hangfire;


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
