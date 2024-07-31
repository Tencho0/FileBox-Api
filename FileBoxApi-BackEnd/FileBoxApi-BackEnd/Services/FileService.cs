namespace FileBoxApi_BackEnd.Services
{
    using System.Threading.Tasks;
    using System.Collections.Generic;

    using Microsoft.EntityFrameworkCore;

    using FileBoxApi.Data;
    using FileBoxApi.Models;

    public class FileService : IFileService
    {
        private readonly FileDbContext data;

        public FileService(FileDbContext data)
        {
            this.data = data;
        }

        public async Task<FileRecord> AddFileAsync(FileRecord file)
        {
            this.data.Files.Add(file);
            await this.data.SaveChangesAsync();
            return file;
        }

        public async Task<bool> DeleteFileAsync(int id)
        {
            var file = await this.data.Files.FindAsync(id);

            if (file == null)
                return false;

            this.data.Files.Remove(file);
            await this.data.SaveChangesAsync();
            return true;
        }

        public async Task<bool> FileExistsAsync(string name, string extension)
        {
            return await this.data.Files.AnyAsync(f => f.Name == name && f.Extension == extension);
        }

        public async Task<IEnumerable<FileRecord>> GetFilesAsync()
        {
            return await this.data.Files
                .OrderBy(f => f.UploadedAt)
                .ToListAsync();
        }
    }
}
