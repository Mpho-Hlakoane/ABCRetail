using Azure.Storage.Files.Shares;
using Microsoft.Extensions.Configuration;

namespace ABCRetail.Services
{
    public class FileStorageService
    {
        private readonly ShareClient _shareClient;

        public FileStorageService(IConfiguration configuration)
        {
            var connectionString = configuration["AzureStorage:ConnectionString"];
            var shareName = configuration["AzureStorage:FileShareName"] ?? "logs";

            _shareClient = new ShareClient(connectionString, shareName);
            _shareClient.CreateIfNotExists();
        }

        // Write or overwrite a log file
        public async Task WriteLogAsync(string fileName, string content)
        {
            var directory = _shareClient.GetRootDirectoryClient();
            var fileClient = directory.GetFileClient(fileName);

            byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(content);
            using (var stream = new MemoryStream(byteArray))
            {
                if (await fileClient.ExistsAsync())
                {
                    await fileClient.DeleteAsync(); // Remove existing file
                }

                await fileClient.CreateAsync(stream.Length);
                await fileClient.UploadAsync(stream);
            }
        }

        // List all files in the share
        public async Task<List<string>> GetAllFilesAsync()
        {
            var files = new List<string>();
            var directory = _shareClient.GetRootDirectoryClient();

            await foreach (var file in directory.GetFilesAndDirectoriesAsync())
            {
                if (!file.IsDirectory)
                    files.Add(file.Name);
            }

            return files;
        }
    }
}
