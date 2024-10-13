using Microsoft.AspNetCore.Mvc;
using PrintingSystem.Db.Interfaces;
using PrintingSystem.Db.Models;
using PrintingSystemAPI.Models;

namespace PrintingSystemAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SessionController : Controller
    {
        private readonly ISessionRepository sessionRepository;

        public SessionController(ISessionRepository sessionRepository)
        {
            this.sessionRepository = sessionRepository;
        } 

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SessionCreateDTO sessionDto)
        {
            try
            {
                var session = new Session()
                {
                    TaskName = sessionDto.TaskName,
                    EmployeeId = sessionDto.EmployeeId,
                    NumberOfPages = sessionDto.NumberOfPages
                };
                var sessionStatus = await sessionRepository.Create(session, sessionDto.DeviceOrderNumber) ? "Успех" : "Неудача";
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
    }
}
