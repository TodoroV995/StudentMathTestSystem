using Application.DTOs.Students;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentsController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var students = await _studentService.GetAllAsync();

            return Ok(students);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var student = await _studentService.GetByIdAsync(id);

            if (student == null)
                return NotFound();

            return Ok(student);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateStudentDto dto)
        {
            var student = await _studentService.CreateAsync(dto);

            return CreatedAtAction(nameof(GetById), new { id = student.Id }, student);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStudent(int id, UpdateStudentDto dto)
        {
            await _studentService.UpdateStudentAsync(id, dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            await _studentService.DeleteStudentAsync(id);
            return NoContent();
        }

        [HttpPost("import-xml")]
        public async Task<ActionResult<ImportStudentsResultDto>> ImportStudentsFromXml(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("XML file is required.");

            using var stream = file.OpenReadStream();

            var result = await _studentService.ImportStudentsFromXmlAsync(stream);

            return Ok(result);
        }
    }
}