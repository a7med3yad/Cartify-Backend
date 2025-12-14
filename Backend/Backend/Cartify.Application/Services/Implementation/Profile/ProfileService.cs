using Cartify.Application.Services.Interfaces;
using Cartify.Domain.Interfaces.Repositories;
using Cartify.Domain.Models;

namespace Cartify.Application.Services.Implementation
{
    public class ProfileService : IProfileService
    {
        private readonly IProfileRepository _profileRepository;

        public ProfileService(IProfileRepository profileRepository)
        {
            _profileRepository = profileRepository;
        }

        public async Task<TblUser> GetUserProfileAsync(int userId)
        {
            return await _profileRepository.GetUser(userId);
        }

        public async Task<bool> UpdateUserProfileAsync(TblUser user)
        {
            try
            {
                await _profileRepository.UpdateAsync(user);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}

