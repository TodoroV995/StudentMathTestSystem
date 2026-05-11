using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExamUploadsController : ControllerBase
    {
        private readonly IExamProcessingService _examProcessingService;

        public ExamUploadsController(IExamProcessingService examProcessingService)
        {
            _examProcessingService = examProcessingService;
        }

        [HttpPost("xml")]
        public async Task<IActionResult> UploadXml(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("XML file is required.");
            }

            if (!file.FileName.EndsWith(".xml", StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest("Only XML files are supported.");
            }

            using var stream = file.OpenReadStream();

            var result = await _examProcessingService.ProcessXmlAsync(stream);

            return Ok(result);
        }
    }
}