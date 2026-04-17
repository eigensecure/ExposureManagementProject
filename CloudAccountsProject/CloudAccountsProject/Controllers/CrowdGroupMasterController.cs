using CloudAccountsProject.Repositories.Contracts;
using CloudAccountsShared.Models;
using Microsoft.AspNetCore.Mvc;

namespace CloudAccountsProject.Controllers
{
    public class CrowdGroupMasterController : BaseApiController
    {
        private readonly ICrowdGroupMasterRepository _repository;

        public CrowdGroupMasterController(ICrowdGroupMasterRepository repository)
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
        public async Task<IActionResult> Create([FromBody] CrowdGroupMaster group)
        {
            var result = await _repository.CreateAsync(group);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CrowdGroupMaster group)
        {
            if (id != group.Id)
                return BadRequest();

            var result = await _repository.UpdateAsync(group);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _repository.DeleteAsync(id);
            return Ok();
        }
    }
}