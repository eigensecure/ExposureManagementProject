using CloudAccountsProject.Controllers;
using CloudAccountsProject.Repositories.Contracts;
using CloudAccountsShared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CloudAccountsProject.Controllers;

public class CloudAccountsController : BaseApiController
{
    private readonly ICloudAccountRepository _repository;
    private readonly IWebHostEnvironment _environment;

    public CloudAccountsController(
        ICloudAccountRepository repository,
        IWebHostEnvironment environment)
    {
        _repository = repository;
        _environment = environment;
    }

    [Authorize]
    [HttpPost("import/{provider}")]
    public async Task<IActionResult> Import(string provider)
    {
        var filePath = Path.Combine(
            _environment.ContentRootPath,
            "jsondata",
            $"{provider.ToLower()}_accounts.json");

        if (!System.IO.File.Exists(filePath))
        {
            return NotFound($"File not found: {filePath}");
        }

        var json = await System.IO.File.ReadAllTextAsync(filePath);

        await _repository.ImportAsync(provider, json);

        return Ok($"{provider} accounts imported successfully.");
    }
}