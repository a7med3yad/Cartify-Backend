using Cartify.Domain.Models;

namespace Cartify.Domain.Interfaces.Repositories
{
    public interface IProfileRepository : IRepository<TblUser>
    {
        Task<TblUser> GetUser(int id);
        Task UpdateAsync(TblUser user);
    }
}