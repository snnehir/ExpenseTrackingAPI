using Hangfire;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTrackingApp.Services.Services.Mail
{
	[AutomaticRetry(Attempts = 0)]
	public interface IEmailService
	{
		Task<BaseResponseModel<EmailResponse>> SendRegisterEmailWithPassword(string name, string lastName, string email, string password);
	}
}
