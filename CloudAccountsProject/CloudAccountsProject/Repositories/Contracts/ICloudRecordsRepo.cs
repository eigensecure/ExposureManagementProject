using CloudAccountsShared.Models.DTOs;

namespace CloudAccountsProject.Repositories.Contracts;
public interface ICloudRecordsRepo
{
    //Task<List<CloudAccountDetailsDTO>> GetCloudAccountDetailsAsync();
    Task SaveBusManDetailsAsync(CloudAccountDetailsDTO item);
    Task<(List<CloudAccountDetailsDTO> CloudAccounts, List<CloudAccountColumnMetadata> ColumnMetadata)> GetCloudAccountDetailsAsync();
}
