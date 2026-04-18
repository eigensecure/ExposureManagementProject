using CloudAccountsProject.Repositories.Contracts;
using CloudAccountsShared.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace CloudAccountsProject.Controllers;

public class CloudRecordsController(ICloudRecordsRepository cloudAccountRepo) : BaseApiController
{
    private readonly ICloudRecordsRepository _cloudAccountRepo = cloudAccountRepo;

    [Authorize]
    [HttpGet("details")]
    public async Task<IActionResult> GetCloudAccountDetails()
    {
        var (cloudAccounts, columnMetadata) = await _cloudAccountRepo.GetCloudAccountDetailsAsync();

        return Ok(new
        {
            CloudAccounts = cloudAccounts,
            ColumnMetadata = columnMetadata
        });
    }

    [Authorize]
    [HttpPost("savedetails")]
    public async Task<IActionResult> SaveBusManDetailsAsync([FromBody] CloudAccountDetailsDTO item)
    {
        try
        {
            await _cloudAccountRepo.SaveBusManDetailsAsync(item);

            return Ok("Updated Successfully");
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"An error occurred while saving the details: {ex.Message}");
        }
    }
}