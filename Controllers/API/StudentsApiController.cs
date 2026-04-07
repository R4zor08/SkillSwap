using Microsoft.AspNetCore.Mvc;
using SkillSwap.Services.Interfaces;
using SkillSwap.Models;

namespace SkillSwap.Web.Controllers.API
{
    [ApiController]
    [Route("api/students")]
    [Route("api/[controller]")]
    public class StudentsApiController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentsApiController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        // GET: api/StudentsApi
        [HttpGet]
        public IActionResult GetAllStudents()
        {
            var students = _studentService.GetAllStudents();
            return Ok(students);
        }

        // GET: api/StudentsApi/{id}
        [HttpGet("{id}")]
        public IActionResult GetStudentById(Guid id)
        {
            var student = _studentService.GetStudentById(id);
            if (student == null)
            {
                return NotFound();
            }
            return Ok(student);
        }

        // POST: api/StudentsApi
        [HttpPost]
        public IActionResult CreateStudent([FromBody] CreateStudentRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _studentService.CreateStudent(request.Name, request.Email, request.Password);
            return CreatedAtAction(nameof(GetStudentById), new { id = Guid.NewGuid() }, request); // Note: In real implementation, return the created student ID
        }

        // PUT: api/StudentsApi/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateStudent(Guid id, [FromBody] UpdateStudentRequest request)
        {
            var student = _studentService.GetStudentById(id);
            if (student == null)
            {
                return NotFound();
            }

            _studentService.UpdateStudent(id, request.Name, request.Email);
            return NoContent();
        }

        // DELETE: api/StudentsApi/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteStudent(Guid id)
        {
            var student = _studentService.GetStudentById(id);
            if (student == null)
            {
                return NotFound();
            }

            _studentService.DeleteStudent(id);
            return NoContent();
        }
    }

    public class CreateStudentRequest
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class UpdateStudentRequest
    {
        public string Name { get; set; }
        public string Email { get; set; }
    }
}