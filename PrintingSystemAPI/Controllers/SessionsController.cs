using Microsoft.AspNetCore.Mvc;
using PrintingSystem.Db.Interfaces;
using PrintingSystem.Db.Models;
using PrintingSystemAPI.Models;

namespace PrintingSystemAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SessionsController : ControllerBase
    {
        private readonly ISessionRepository sessionRepository;

        public SessionsController(ISessionRepository sessionRepository)
        {
            this.sessionRepository = sessionRepository;
        } 

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] SessionCreateDTO sessionDto)
        {
            try
            {
                var session = new Session()
                {
                    TaskName = sessionDto.TaskName,
                    EmployeeId = sessionDto.EmployeeId,
                    NumberOfPages = sessionDto.NumberOfPages
                };
                var sessionStatus = await sessionRepository.CreateAsync(session, sessionDto.DeviceOrderNumber) ? "Успех" : "Неудача";
                return Ok(new { sessionStatus });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while processing the request.", error = ex.Message });
            }
        }

        [HttpPost("upload-csv")]
        public async Task<IActionResult> UploadPrintJobsFromCsvAsync(IFormFile file)
        {
            try
            {
                var result = await sessionRepository.ProcessSessionsFromCsvAsync(file);
                return Ok(new { result });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return UnprocessableEntity(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while processing the file.", error = ex.Message });
            }
        }
    }
}
