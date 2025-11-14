using Cartify.Domain.Entities;

namespace Cartify.API.Contracts
{
	public class DtoSubmitTicket
	{
		public string Name { get; set; }
		public string Email { get; set; }
		public IssueCategory IssueCategory { get; set; }
		public string Subject { get; set; }
		public string Message { get; set; }
	}
}
