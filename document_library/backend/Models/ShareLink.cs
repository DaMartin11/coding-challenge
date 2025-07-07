namespace backend.Models
{
    public class ShareLink
    {
        public Guid Token { get; set; } = Guid.NewGuid();

        public Guid DocumentId { get; set; }

        public DateTime ExpiresAt { get; set; }
         public bool IsExpired => DateTime.UtcNow > ExpiresAt;
    }
}