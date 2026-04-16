using CloudAccountsProject.Repositories.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace CloudAccountsProject.Controllers;

public class CloudHistoryController : BaseApiController
{
    private readonly ICloudHistoryRepository _historyRepository;

    public CloudHistoryController(ICloudHistoryRepository repository)
    {
        _historyRepository = repository;
    }

    [HttpGet("auditMaster/{accId}")]
    public async Task<IActionResult> GetAuditByAccId(string accId)
    {
        var result =  await _historyRepository.GetAuditByAccId(accId);
        return Ok(result);
    }

}