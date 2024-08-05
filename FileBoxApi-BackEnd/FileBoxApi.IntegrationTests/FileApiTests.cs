namespace FileBoxApi.IntegrationTests
{
    using Xunit;

    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text.Json;
    using System.Threading.Tasks;
    using System.Collections.Generic;

    using FileBoxApi.Models;

    public class FileApiTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly JsonSerializerOptions _jsonOptions;
        private readonly string _uploadedFileName;

        public FileApiTests(CustomWebApplicationFactory<Program> factory)
        {
            this._client = factory.CreateClient();

            this._jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            this._uploadedFileName = $"testfile_{Guid.NewGuid()}.txt";
        }

        [Fact]
        public async Task GetFiles_ReturnsOkResponse()
        {
            // Act
            var response = await _client.GetAsync("/api/files");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task UploadFiles_ReturnsOkResponse()
        {
            // Arrange
            var formContent = new MultipartFormDataContent();
            var fileContent = new ByteArrayContent(System.Text.Encoding.UTF8.GetBytes("This is a dummy file."));
            fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
            {
                Name = "files",
                FileName = this._uploadedFileName
            };

            formContent.Add(fileContent);

            // Act
            var response = await _client.PostAsync("/api/files/upload", formContent);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task DeleteFile_ReturnsNoContentResponse()
        {
            var formContent = new MultipartFormDataContent();
            var fileContent = new ByteArrayContent(System.Text.Encoding.UTF8.GetBytes("This is a dummy file."));
            fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
            {
                Name = "files",
                FileName = this._uploadedFileName
            };

            formContent.Add(fileContent);

            var uploadResponse = await _client.PostAsync("/api/files/upload", formContent);
            uploadResponse.EnsureSuccessStatusCode();

            var filesResponse = await _client.GetAsync("/api/files");
            filesResponse.EnsureSuccessStatusCode();
            var filesJson = await filesResponse.Content.ReadAsStringAsync();
            var files = JsonSerializer.Deserialize<List<FileRecord>>(filesJson, _jsonOptions);

            Assert.NotNull(files);
            Assert.NotEmpty(files);

            var fileId = files[0].Id;

            // Act
            var deleteResponse = await _client.DeleteAsync($"/api/files/{fileId}");

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);
        }
    }
}