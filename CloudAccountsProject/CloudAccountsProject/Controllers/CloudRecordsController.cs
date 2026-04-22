using CloudAccountsProject.Repositories.Contracts;
using CloudAccountsShared.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace CloudAccountsProject.Controllers;

public class CloudRecordsController(ICloudRecordsRepository cloudAccountRepo, IBlobStorageRepository blobStorageRepository) : BaseApiController
{
    private readonly ICloudRecordsRepository _cloudAccountRepo = cloudAccountRepo;
    private readonly IBlobStorageRepository _blobStorageRepository = blobStorageRepository;

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

    public class UploadAttachmentRequest
    {
        public IFormFile file { get; set; }
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

    [Authorize]
    [HttpPost("uploadattachment")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UploadAttachment([FromForm] UploadAttachmentRequest attachment, [FromForm] string cloudAccountId)
    {
        try
        {
            if (attachment.file == null || attachment.file.Length == 0)
                return BadRequest("No file selected.");

            var blobPath = await _blobStorageRepository
                .UploadCloudRecordAttachmentAsync(attachment.file, cloudAccountId);

            return Ok(blobPath);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"An error occurred while uploading the file: {ex.Message}");
        }
    }
}