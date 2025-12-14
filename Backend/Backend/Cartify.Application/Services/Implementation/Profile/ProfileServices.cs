using Cartify.Application.Services.Interfaces.Product;
using Cartify.Domain.Interfaces.Repositories;
using Cartify.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Cartify.Application.Services.Implementation.Profile
{
    public class ProfileServices : IProfileServices
    {
        private readonly IProfileRepository _profileRepository;

        public ProfileServices(IProfileRepository profileRepository)
        {
            _profileRepository = profileRepository;
        }

        public async Task<TblUser> GetUsersProfilePicture(int id)
        {
            return await _profileRepository.GetUser(id);
        }
    }
}
