using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace DocumentLibraryApi.Storage
{
    public interface IFileStorage
    {
        Task<string> SaveFileAsync(IFormFile file, string folder);
    }
}
