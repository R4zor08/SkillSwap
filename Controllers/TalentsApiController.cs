using Microsoft.AspNetCore.Mvc;
using SkillSwap.Services.Interfaces;
using SkillSwap.Models;

namespace SkillSwap.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TalentsApiController : ControllerBase
    {
        private readonly ITalentService _talentService;

        public TalentsApiController(ITalentService talentService)
        {
            _talentService = talentService;
        }

        // GET: api/TalentsApi
        [HttpGet]
        public IActionResult GetAllTalents()
        {
            var talents = _talentService.GetTalents();
            return Ok(talents);
        }

        // GET: api/TalentsApi/{id}
        [HttpGet("{id}")]
        public IActionResult GetTalentById(Guid id)
        {
            var talent = _talentService.GetTalentById(id);
            if (talent == null)
            {
                return NotFound();
            }
            return Ok(talent);
        }

        // GET: api/TalentsApi/student/{studentId}
        [HttpGet("student/{studentId}")]
        public IActionResult GetTalentsByStudent(Guid studentId)
        {
            var talents = _talentService.GetTalentsByStudent(studentId);
            return Ok(talents);
        }

        // POST: api/TalentsApi
        [HttpPost]
        public IActionResult CreateTalent([FromBody] CreateTalentRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var talent = _talentService.AddTalent(request.Name, request.Description, request.StudentId);
            return CreatedAtAction(nameof(GetTalentById), new { id = talent.TalentId }, talent);
        }

        // PUT: api/TalentsApi/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateTalent(Guid id, [FromBody] UpdateTalentRequest request)
        {
            var talent = _talentService.GetTalentById(id);
            if (talent == null)
            {
                return NotFound();
            }

            _talentService.UpdateTalent(id, request.Name, request.Description, request.ProficiencyLevel);
            return NoContent();
        }

        // DELETE: api/TalentsApi/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteTalent(Guid id)
        {
            var talent = _talentService.GetTalentById(id);
            if (talent == null)
            {
                return NotFound();
            }

            _talentService.DeleteTalent(id);
            return NoContent();
        }
    }

    public class CreateTalentRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid StudentId { get; set; }
    }

    public class UpdateTalentRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int ProficiencyLevel { get; set; }
    }
}