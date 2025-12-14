using System;
using Cartify.Domain.Models;

namespace Cartify.Application.Services.Interfaces
{
    public interface IProfileService
    {
        Task<TblUser> GetUserProfileAsync(int userId);
        Task<bool> UpdateUserProfileAsync(TblUser user);
    }
}
// 1 - IXRepo 
// 2 - XRepo
// 3 - Dependcy injection IUnitofWork !!
// 4 - Dependcy injecttion UnitofWork !!
// 5 - IXServices
// 6 - foledr -> XServices
// 7 - Dependcy injection in Program.cs !!
// 8 - XController !!
// 9 - Test on Swagger !!


