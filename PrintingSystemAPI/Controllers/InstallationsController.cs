using Microsoft.AspNetCore.Mvc;
using PrintingSystem.Db.Interfaces;
using PrintingSystem.Db.Models;
using PrintingSystemAPI.Models;

namespace PrintingSystemAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InstallationsController : ControllerBase
    {
        private readonly IInstallationRepository installationRepository;

        public InstallationsController(IInstallationRepository installationRepository)
        {
            this.installationRepository = installationRepository;
        }

        /// <summary>
        /// Создает новую инсталляцию устройства печати.
        /// </summary>
        /// <param name="installationDto">Данные для создания инсталляции.</param>
        /// <returns>Созданная инсталляция.</returns>
        /// <response code="200">Возвращает идентификатор созданной инсталляции.</response>
        /// <response code="400">Некорректные данные в запросе.</response>
        /// <response code="409">Конфликт с существующими данными.</response>
        /// <response code="500">Ошибка на сервере.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateAsync([FromBody] InstallationCreateDTO installationDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var installation = new Installation
                {
                    Name = installationDto.Name,
                    OfficeId = installationDto.OfficeId,
                    PrintingDeviceId = installationDto.PrintingDeviceId,
                    InstallationOrderNumber = installationDto.DeviceOrderNumber,
                    IsDefault = installationDto.IsDefault
                };

                var createdInstallationId = await installationRepository.CreateAsync(installation);

                return Ok( new { CreatedInstallationId = createdInstallationId } );
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }

        /// <summary>
        /// Получает инсталляцию по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор инсталляции.</param>
        /// <returns>Информация о инсталляции.</returns>
        /// <response code="200">Возвращает информацию об инсталляции.</response>
        /// <response code="404">Инсталляция не найдена.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var installation = await installationRepository.GetByIdAsync(id);

            if (installation == null)
                return NotFound(new { message = "Installation not found." });

            var installationDTO = new InstalationDTO()
            {
                Id = installation.Id,
                Name = installation.Name,
                OfficeId = installation.OfficeId,
                PrintingDeviceId = installation.PrintingDeviceId,
                InstallationOrderNumber = installation.InstallationOrderNumber,
                IsDefault = installation.IsDefault
            };

            return Ok(installationDTO);
        }

        /// <summary>
        /// Получает все инсталляции или инсталляции по идентификатору филиала.
        /// </summary>
        /// <param name="officeId">Идентификатор филиала (необязательный).</param>
        /// <returns>Список инсталляций.</returns>
        /// <response code="200">Возвращает список инсталляций.</response>
        /// <response code="404">Инсталляции не найдены.</response>
        /// <response code="500">Ошибка на сервере.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<InstalationDTO>>> GetAllByOfficeIdAsync([FromQuery] Guid? officeId)
        {
            try
            {
                IEnumerable<Installation> installations = officeId.HasValue ?
                await installationRepository.GetByOfficeIdAsync(officeId.Value) :
                await installationRepository.GetAllAsync();

                var installationDtos = installations.Select(inst => new InstalationDTO()
                {
                    Id = inst.Id,
                    Name = inst.Name,
                    OfficeId = inst.OfficeId,
                    PrintingDeviceId = inst.PrintingDeviceId,
                    InstallationOrderNumber = inst.InstallationOrderNumber,
                    IsDefault = inst.IsDefault
                });

                return Ok(installationDtos);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while deleting the installation.", details = ex.Message });
            }
        }

        /// <summary>
        /// Удаляет инсталляцию по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор инсталляции.</param>
        /// <returns>Результат удаления.</returns>
        /// <response code="204">Инсталляция успешно удалена.</response>
        /// <response code="404">Инсталляция не найдена.</response>
        /// <response code="409">Конфликт с существующими данными.</response>
        /// <response code="500">Ошибка на сервере.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteAsync(Guid id)
        {
            try
            {
                await installationRepository.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while deleting the installation.", details = ex.Message });
            }
        }
    }
}
