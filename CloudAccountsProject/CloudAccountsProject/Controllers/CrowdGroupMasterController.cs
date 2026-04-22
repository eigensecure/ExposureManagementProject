using CloudAccountsProject.Repositories.Contracts;
using CloudAccountsShared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CloudAccountsProject.Controllers
{
    public class CrowdGroupMasterController(ICrowdGroupMasterRepository repository) : BaseApiController
    {
        private readonly ICrowdGroupMasterRepository _repository = repository;

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _repository.GetAllAsync();
            return Ok(result);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _repository.GetByIdAsync(id);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CrowdGroupMaster group)
        {
            try
            {
                var result = await _repository.CreateAsync(group);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CrowdGroupMaster group)
        {
            if (id != group.Id)
                return BadRequest();

            var result = await _repository.UpdateAsync(group);
            return Ok(result);
        }

        [HttpGet("accountids")]
        public async Task<IActionResult> GetAllAccountIds([FromQuery] int businessFunctionId, [FromQuery] string provider)
        {
            var result = await _repository.GetAllAccountIdsAsync(businessFunctionId, provider);

            return Ok(result);
        }

        [HttpPost("patch/{id}")]
        public async Task<IActionResult> PatchGroup(int id)
        {
            try
            {
                await _repository.PatchGroupAsync(id);

                return Ok("Patched successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _repository.DeleteAsync(id);
            return Ok();
        }
    }
}