namespace FileBoxApi.IntegrationTests
{
    using FileBoxApi.Data;
    using FileBoxApi.Models;

    public static class SeedData
    {
        public static void PopulateTestData(FileDbContext db)
        {
            if (db.Files.Any())
            {
                return;
            }

            db.Files.AddRange(
                new FileRecord
                {
                    Name = "example1",
                    Extension = "txt",
                    Content = System.Text.Encoding.UTF8.GetBytes("This is example file 1."),
                    UploadedAt = DateTime.UtcNow.AddDays(-2)
                },
                new FileRecord
                {
                    Name = "example2",
                    Extension = "txt",
                    Content = System.Text.Encoding.UTF8.GetBytes("This is example file 2."),
                    UploadedAt = DateTime.UtcNow.AddDays(-1)
                }
            );

            db.SaveChanges();
        }
    }
}
