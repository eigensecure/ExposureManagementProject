using CloudAccountsShared.Models.DTOs;

namespace CloudAccountsProject.Repositories.Contracts;
public interface ICloudRecordsRepository
{
    Task<(List<CloudAccountDetailsDTO> CloudAccounts, List<CloudAccountColumnMetadata> ColumnMetadata)> GetCloudAccountDetailsAsync();

    Task SaveBusManDetailsAsync(CloudAccountDetailsDTO item);
}
