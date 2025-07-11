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

        public List<string> GetFiles(string folder)
        {
            var folderPath = Path.Combine(_rootPath, folder);

            if (!Directory.Exists(folderPath))
            {
                return new List<string>(); // Or throw exception depending on your needs
            }

            var files = Directory.GetFiles(folderPath);

            // Return relative paths
            return files.Select(filePath =>
            {
                var fileName = Path.GetFileName(filePath);
                return $"/{folder}/{fileName}";
            }).ToList();
        }
    }
}
