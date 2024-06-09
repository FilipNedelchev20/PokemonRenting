using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonRenting.Repositories.Infrastructure
{
    public interface IFileStorage
    {
        Task<string> SaveFile(IFormFile file, string containerName);
        Task RemoveFile(string fileRoute, string containerName);
        Task<string> EditFile(IFormFile file, string containerName, string fileRoute);
    }
}
