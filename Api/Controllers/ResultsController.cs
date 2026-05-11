using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResultsController : ControllerBase
    {
        private readonly IResultService _resultService;

        public ResultsController(IResultService resultService)
        {
            _resultService = resultService;
        }

        [HttpGet("student/{studentId}")]
        public async Task<IActionResult> GetStudentResults(int studentId)
        {
            var results = await _resultService.GetResultsForStudentAsync(studentId);

            return Ok(results);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllResults()
        {
            var results = await _resultService.GetAllResultsAsync();

            return Ok(results);
        }
    }
}