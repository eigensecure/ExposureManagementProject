using CloudAccountsShared.Models.DTOs;

namespace CloudAccountsUI.Services.Contracts;
public interface ICloudRecordsService
{
    Task<(List<CloudAccountDetailsDTO> CloudAccounts, List<CloudAccountColumnMetadata> ColumnMetadata)> GetCloudAccountDetailsAsync();

    Task SaveBusManDetailsAsync(CloudAccountDetailsDTO item);
}