using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

namespace DocumentLibraryApi.Storage
{
    public class LocalFileStorage : IFileStorage
    {
        private readonly string _rootPath;

        public LocalFileStorage()
        {
            _rootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            Directory.CreateDirectory(_rootPath);
        }

        public async Task<string> SaveFileAsync(IFormFile file, string folder)
        {
            var folderPath = Path.Combine(_rootPath, folder);
            Directory.CreateDirectory(folderPath);

            var filePath = Path.Combine(folderPath, file.FileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Return relative path
            return $"/{folder}/{file.FileName}";
        }
    }
}
