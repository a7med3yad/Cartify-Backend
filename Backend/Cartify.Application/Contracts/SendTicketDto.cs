using Cartify.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cartify.Application.Contracts
{
	public class SendTicketDto
	{
		public string SenderName { get; set; } = "Cartify Support";

		public string SenderEmail { get; set; } = "999cb7001@smtp-brevo.com";
		public string Name { get; set; }
		public string Email { get; set; }
		public IssueCategory IssueCategory { get; set; }
		public string Subject { get; set; }
		public string Message { get; set; }
	}
}
