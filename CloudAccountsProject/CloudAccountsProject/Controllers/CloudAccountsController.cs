using CloudAccountsProject.Controllers;
using CloudAccountsProject.Repositories.Contracts;
using CloudAccountsShared.Models;
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

    //[HttpPut("{id}")]
    //public async Task<IActionResult> Update(int id, CloudAccount account)
    //{
    //    if (id != account.Id)
    //    {
    //        return BadRequest();
    //    }

    //    await _repository.UpdateAsync(account);

    //    return Ok("Updated successfully.");
    //}

    [HttpGet("column-metadata")]
    public async Task<IActionResult> GetColumnMetadata()
    {
        var result = await _repository.GetColumnMetadataAsync();
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _repository.DeleteAsync(id);

        return Ok("Deleted successfully.");
    }
}