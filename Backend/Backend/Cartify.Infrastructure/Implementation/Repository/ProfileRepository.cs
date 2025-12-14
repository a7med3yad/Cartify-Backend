using Cartify.Domain.Interfaces.Repositories;
using Cartify.Domain.Models;
using Cartify.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Cartify.Infrastructure.Implementation.Repository
{
    public class ProfileRepository : Repository<TblUser>, IProfileRepository
    {
        private readonly AppDbContext _context;

        public ProfileRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<TblUser> GetUser(int id)
        {
            return await _context.TblUsers
                .Include(u => u.TblAddresses)
                .FirstOrDefaultAsync(u => u.Id == id.ToString());
        }

        public async Task UpdateAsync(TblUser user)
        {
            _context.TblUsers.Update(user);
            await _context.SaveChangesAsync();
        }
    }
}