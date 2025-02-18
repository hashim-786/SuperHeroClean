using Microsoft.AspNetCore.Mvc;
using SuperHeroAPI.Application.DTOs;
using SuperHeroAPI.Core.Entities;
using SuperHeroAPI.Core.Interfaces;

namespace SuperHeroAPI.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModuleController : ControllerBase
    {
        private readonly IModuleRepository _moduleRepository;

        public ModuleController(IModuleRepository moduleRepository)
        {
            _moduleRepository = moduleRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Module>>> GetModules()
        {
            var modules = await _moduleRepository.GetAllModulesAsync();
            return Ok(modules);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Module>> GetModule(int id)
        {
            var module = await _moduleRepository.GetModuleByIdAsync(id);
            if (module == null)
                return NotFound("Module not found.");
            return Ok(module);
        }

        [HttpPost]
        public async Task<ActionResult<Module>> CreateModule([FromBody] ModuleDto moduleDto)
        {
            if (string.IsNullOrWhiteSpace(moduleDto.Name))
                return BadRequest("Module name is required.");

            var module = new Module { Name = moduleDto.Name };
            var createdModule = await _moduleRepository.AddModuleAsync(module);
            return CreatedAtAction(nameof(GetModule), new { id = createdModule.Id }, createdModule);
        }
    }
}
