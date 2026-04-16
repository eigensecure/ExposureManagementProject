using CloudAccountsProject.Repositories.Contracts;
using CloudAccountsShared.Models;
using Microsoft.AspNetCore.Mvc;

namespace CloudAccountsProject.Controllers;

public class BusinessFunctionController : BaseApiController
{
    private readonly IBusinessFunctionRepository _repository;

    public BusinessFunctionController(IBusinessFunctionRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _repository.GetAllAsync();
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _repository.GetByIdAsync(id);

        if (result == null)
            return NotFound();

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(BusinessFunction item)
    {
        try
        {
            await _repository.CreateAsync(item);
            return Ok(item);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, BusinessFunction item)
    {
        if (id != item.Id)
            return BadRequest();

        try
        {
            await _repository.UpdateAsync(item);
            return Ok(item);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}