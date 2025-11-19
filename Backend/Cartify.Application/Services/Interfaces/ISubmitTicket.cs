using Cartify.Application.Contracts;

namespace Cartify.Application.Services.Interfaces
{
	public interface ISubmitTicket
	{
		Task<bool> SendTicket(SendTicketDto sendTicket);
	}
}
