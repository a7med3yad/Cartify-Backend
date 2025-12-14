using Cartify.Application.Contracts.AuthenticationDtos;
using Cartify.Domain.Models;

namespace Cartify.Application.Services.Interfaces.Authentication
{
	public interface IRegisterService
	{
		Task<string> Register(dtoRegister register);

	}
}
