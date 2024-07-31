namespace FileBoxApi_BackEnd.Services
{
    using FileBoxApi.Models;

    public interface IFileService
    {
        Task<IEnumerable<FileRecord>> GetFilesAsync();
        
        Task<FileRecord> AddFileAsync(FileRecord file);
        
        Task<bool> DeleteFileAsync(int id);

        Task<bool> FileExistsAsync(string name, string extension);
    }
}
