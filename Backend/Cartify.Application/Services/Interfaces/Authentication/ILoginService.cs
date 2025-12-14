using Cartify.Application.Contracts.AuthenticationDtos;

namespace Cartify.Application.Services.Interfaces.Authentication
{
	public interface ILoginService
	{
		Task<dtoTokenResult> Login(dtoLogin login);
		Task<dtoTokenResult> RefreshToken(string  token);

	}
}
