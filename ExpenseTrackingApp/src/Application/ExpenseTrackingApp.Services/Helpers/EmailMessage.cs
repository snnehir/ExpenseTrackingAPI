﻿using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTrackingApp.Services.Helpers
{
	public class EmailMessage
	{
		public List<MailboxAddress> To { get; set; }
		public string Title { get; set; }
		public string Subject { get; set; }
		public string Content { get; set; }
		public EmailMessage(IEnumerable<string> to, string title, string subject, string content)
		{
			To = new List<MailboxAddress>();
			To.AddRange(to.Select(x => new MailboxAddress(x, x)));
			Title = title;
			Subject = subject;
			Content = content;
		}
	}
}
