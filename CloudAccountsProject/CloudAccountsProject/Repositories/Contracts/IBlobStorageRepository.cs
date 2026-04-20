namespace CloudAccountsProject.Repositories.Contracts;
public interface IBlobStorageRepository
{
    Task<string> UploadCloudRecordAttachmentAsync(IFormFile file, string cloudAccountId);
    Task DeleteAsync(string blobPath);
}

