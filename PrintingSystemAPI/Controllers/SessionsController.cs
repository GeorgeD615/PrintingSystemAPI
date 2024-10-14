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

        /// <summary>
        /// Создает новое задание на печать.
        /// </summary>
        /// <param name="sessionDto">Данные для создания задания.</param>
        /// <returns>Статус выполнения задания.</returns>
        /// <response code="200">Возвращает статус задания на печать.</response>
        /// <response code="400">Неверные данные модели.</response>
        /// <response code="500">Ошибка на сервере.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateAsync([FromBody] SessionCreateDTO sessionDto)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var session = new Session()
                {
                    TaskName = sessionDto.TaskName,
                    EmployeeId = sessionDto.EmployeeId,
                    NumberOfPages = sessionDto.NumberOfPages
                };
                var sessionStatus = await sessionRepository.CreateAsync(session, sessionDto.DeviceOrderNumber) ? 
                    "Успех" : 
                    "Неудача";
                return Ok(new { CreatedSessionStatus = sessionStatus });
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

        /// <summary>
        /// Загружает задания на печать из CSV файла.
        /// </summary>
        /// <param name="file">CSV файл с заданиями на печать.</param>
        /// <returns>Количество созданных заданий.</returns>
        /// <response code="200">Возвращает количество созданных заданий.</response>
        /// <response code="400">Неверные данные в запросе.</response>
        /// <response code="422">Файл не соответствует ожидаемому формату.</response>
        /// <response code="500">Ошибка на сервере.</response>
        [HttpPost("upload-csv")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UploadPrintJobsFromCsvAsync(IFormFile file)
        {
            try
            {
                var result = await sessionRepository.ProcessSessionsFromCsvAsync(file);
                return Ok(new { NumberOfSessionsCreated = result });
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
