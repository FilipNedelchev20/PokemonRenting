using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using PokemonRenting.Repositories.Infrastructure;
using PokemonRenting.Repositories.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonRenting.Repositories.Implementation
{
    public class FileStorage : IFileStorage
    {
        private readonly IWebHostEnvironment _env;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public FileStorage(IHttpContextAccessor httpContextAccessor, IWebHostEnvironment env)
        {
            this._httpContextAccessor = httpContextAccessor;
            _env = env;
        }

        public async Task<string> EditFile(IFormFile file, string containerName, string fileRoute)
        {
            await RemoveFile(fileRoute, containerName);
            return await SaveFile(file, containerName);
        }

        public  Task RemoveFile(string fileRoute, string containerName)
        {
            if (string.IsNullOrEmpty(fileRoute))
            {
                return Task.CompletedTask;
            }
            var filename = Path.GetFileName(fileRoute);
            var fileDirectory = Path.Combine(_env.WebRootPath,
                containerName, filename);
            if (File.Exists(fileDirectory))
            {
                File.Delete(fileDirectory);
            }
            return Task.CompletedTask;
        }

        public async Task<string> SaveFile(IFormFile file, string containerName)
        {
            var extention = Path.Combine(file.FileName);
            var filename = $"{Guid.NewGuid()}{extention}";
            string folder =Path.Combine(_env.WebRootPath,containerName);
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            string route = Path.Combine(folder, filename);
            using var memoryStream = new MemoryStream();
           await  file.CopyToAsync(memoryStream);
            var content = memoryStream.ToArray();
            await File.WriteAllBytesAsync(route, content);
            var currentUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}";
            var path = Path.Combine(currentUrl, containerName,filename).Replace("\\", "/");
            return path;
        }
    }
}
