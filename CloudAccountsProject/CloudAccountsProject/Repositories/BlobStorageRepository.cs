using CloudAccountsProject.Repositories.Contracts;
using Azure.Storage.Blobs;
namespace CloudAccountsProject.Repositories;
    public class BlobStorageRepository(IConfiguration configuration) : IBlobStorageRepository
    {
        private readonly BlobContainerClient _containerClient =
            new BlobContainerClient(
                configuration["AzureBlobStorage:ConnectionString"],
                "exposure-management");

        public async Task<string> UploadCloudRecordAttachmentAsync(IFormFile file, string cloudAccountId)
        {
            var folder = $"cloudrecords/{cloudAccountId}";

            var originalFileName = Path.GetFileName(file.FileName);
            var extension = Path.GetExtension(originalFileName);
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(originalFileName);

            var finalFileName = originalFileName;
            var blobPath = $"{folder}/{finalFileName}";

            int count = 1;

            while (await _containerClient.GetBlobClient(blobPath).ExistsAsync())
            {
                finalFileName = $"{fileNameWithoutExtension}({count}){extension}";
                blobPath = $"{folder}/{finalFileName}";
                count++;
            }

            var blobClient = _containerClient.GetBlobClient(blobPath);

            await using var stream = file.OpenReadStream();
            await blobClient.UploadAsync(stream, overwrite: false);

            return blobPath;
        }

        public async Task DeleteAsync(string blobPath)
        {
            if (string.IsNullOrWhiteSpace(blobPath))
                return;

            var blobClient = _containerClient.GetBlobClient(blobPath);

            await blobClient.DeleteIfExistsAsync();
        }
    }