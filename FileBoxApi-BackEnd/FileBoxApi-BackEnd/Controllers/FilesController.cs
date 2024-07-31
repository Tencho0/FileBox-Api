namespace FileBoxApi_BackEnd.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using FileBoxApi.Models;
    using FileBoxApi_BackEnd.Services;

    [ApiController]
    [Route("api/[controller]")]
    public class FilesController : ControllerBase
    {
        private readonly IFileService fileService;

        public FilesController(IFileService fileService)
        {
            this.fileService = fileService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FileRecord>>> GetFiles()
        {
            var files = await this.fileService.GetFilesAsync();
            return Ok(files.Select(f => new { f.Id, f.Name, f.Extension, f.UploadedAt }));
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload([FromForm] List<IFormFile> files)
        {
            foreach (var file in files)
            {
                if (file.Length == 0)
                    return BadRequest("Cannot upload empty files");

                var fileNameParts = Path.GetFileNameWithoutExtension(file.FileName).Split('.');
                var name = string.Join(".", fileNameParts.Take(fileNameParts.Length - 1));
                var extension = fileNameParts.Last() + Path.GetExtension(file.FileName);

                if (await this.fileService.FileExistsAsync(name, extension))
                    return Conflict($"File {name}.{extension} already exists.");

                using var ms = new MemoryStream();
                await file.CopyToAsync(ms);

                var fileRecord = new FileRecord
                {
                    Name = name,
                    Extension = extension,
                    Content = ms.ToArray(),
                    UploadedAt = DateTime.UtcNow
                };

                await this.fileService.AddFileAsync(fileRecord);
            }

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await this.fileService.DeleteFileAsync(id);
            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}
