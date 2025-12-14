using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cartify.Domain.Interfaces.Repositories
{
    public interface IFileStorageService
    {
        Task<string> UploadFileAsync(IFormFile file, string folder);
    }
}
