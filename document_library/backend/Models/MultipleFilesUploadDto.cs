using Microsoft.AspNetCore.Http;

namespace backend.Models
{
    public class MultipleFilesUploadDto
    {
        public List<IFormFile> Files { get; set; }
    }
}