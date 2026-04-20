using CloudAccountsShared.Models.DTOs;
using Microsoft.AspNetCore.Components.Forms;

namespace CloudAccountsUI.Services.Contracts;
public interface ICloudRecordsService
{
    Task<(List<CloudAccountDetailsDTO> CloudAccounts, List<CloudAccountColumnMetadata> ColumnMetadata)> GetCloudAccountDetailsAsync();

    Task SaveBusManDetailsAsync(CloudAccountDetailsDTO item);

    Task<string> UploadAttachmentAsync(IBrowserFile file, string cloudAccountId);
}