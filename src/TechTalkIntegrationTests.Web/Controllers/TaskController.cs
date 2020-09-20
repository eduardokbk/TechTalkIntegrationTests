using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TechTalkIntegrationTests.Domain.Models.Dtos.Tasks;
using TechTalkIntegrationTests.Domain.Models.Services;

namespace TechTalkIntegrationTests.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController : ControllerBase
    {
        private readonly ITaskAppService _appService;
        public TaskController(ITaskAppService appService)
        {
            _appService = appService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _appService.GetAllAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] TaskForCreationDto dto)
        {
            return Ok(await _appService.CreateAsync(dto));
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] TaskForCreationDto dto)
        {
            await _appService.UpdateAsync(id, dto);
            return NoContent();
        }

        [HttpPatch]
        [Route("{id}/complete")]
        public async Task<IActionResult> Patch(Guid id)
        {
            await _appService.CompleteAsync(id);
            return NoContent();
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _appService.DeleteAsync(id);
            return NoContent();
        }
    }
}
