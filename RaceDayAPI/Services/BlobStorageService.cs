
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace RaceDayAPI.Services
{
    public class BlobStorageService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private const string ContainerName = "event-banners";

        public BlobStorageService(IConfiguration configuration)
        {
            var connectionString =
                configuration.GetConnectionString("AzureBlobStorage");

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException(
                    "Azure Blob Storage connection string is missing.");
            }

            _blobServiceClient =
                new BlobServiceClient(connectionString);
        }

        public async Task<string> UploadImageAsync(
            Stream fileStream,
            string fileName,
            string contentType)
        {
            var containerClient =
                _blobServiceClient.GetBlobContainerClient(ContainerName);

            await containerClient.CreateIfNotExistsAsync();

            var blobClient =
                containerClient.GetBlobClient(fileName);

            await blobClient.UploadAsync(
                fileStream,
                new BlobHttpHeaders
                {
                    ContentType = contentType
                });

            return blobClient.Uri.ToString();
        }
    }
}
