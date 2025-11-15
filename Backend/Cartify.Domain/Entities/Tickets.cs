namespace Cartify.Domain.Entities
{
	public enum TicketStatus
	{
		Open,
		Pending,
		Closed
	}
	public enum IssueCategory
	{
		OrderIssue,
		ShippingAndDelivery,
		ReturnsAndRefunds,
		ProductQuestions,
		AccountIssues,
		Other
	}

	public class Ticket
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Email { get; set; }
		public IssueCategory IssueCategory {  get; set; }
		public string Subject {  get; set; }
		public string Message { get; set; }
		public DateTime CreatedAt { get; set; }= DateTime.Now;
		public TicketStatus TicketStatus {  get; set; } = TicketStatus.Open;
		public DateTime? ClosedAt {  get; set; }


	}
}
