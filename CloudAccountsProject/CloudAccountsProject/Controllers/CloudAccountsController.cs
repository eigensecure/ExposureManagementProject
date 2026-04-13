using CloudAccountsProject.Models;
using CloudAccountsProject.Repositories.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace CloudAccounts.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CloudAccountsController : ControllerBase
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

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _repository.GetAllAsync();
        return Ok(result);
    }

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

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, CloudAccount account)
    {
        if (id != account.Id)
        {
            return BadRequest();
        }

        await _repository.UpdateAsync(account);

        return Ok("Updated successfully.");
    }
        


    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _repository.DeleteAsync(id);

        return Ok("Deleted successfully.");
    }
}