namespace backend.Models
{
    public class Document
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string Type { get; set; }
        public DateTime UploadDate { get; set; } = DateTime.UtcNow;
        public string FilePath { get; set; }
         public int DownloadCount { get; set; }
        public string PreviewUrl { get; set; } = string.Empty;
        public string IconUrl { get; set; } = string.Empty;
    }
}