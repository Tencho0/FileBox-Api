namespace FileBoxApi.Models
{
    public class FileRecord
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Extension { get; set; } = string.Empty;

        public byte[] Content { get; set; } = Array.Empty<byte>();

        public DateTime UploadedAt { get; set; }
    }
}
