using Microsoft.AspNetCore.Mvc;
using PrintingSystem.Db.Interfaces;
using PrintingSystem.Db.Models;
using PrintingSystemAPI.Models;

namespace PrintingSystemAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InstallationController : Controller
    {
        private readonly IInstallationRepository installationRepository;

        public InstallationController(IInstallationRepository installationRepository)
        {
            this.installationRepository = installationRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CreateInstallation([FromBody] InstallationCreateDTO installationDto)
        {
            if (installationDto == null)
                return BadRequest(new { message = "Installation data is required." });

            if (string.IsNullOrEmpty(installationDto.Name) || installationDto.OfficeId == Guid.Empty || installationDto.PrintingDeviceId == Guid.Empty)
                return BadRequest(new { message = "Invalid data: Name, OfficeId and PrintingDeviceId are required." });

            try
            {
                var installation = new Installation
                {
                    Name = installationDto.Name,
                    OfficeId = installationDto.OfficeId,
                    PrintingDeviceId = installationDto.PrintingDeviceId,
                    InstallationOrderNumber = installationDto.InstallationOrderNumber,
                    IsDefault = installationDto.IsDefault
                };

                var createdInstallationId = await installationRepository.CreateAsync(installation);

                return CreatedAtAction(nameof(GetInstallationById), new { id = createdInstallationId }, new { id = createdInstallationId });
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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetInstallationById(Guid id)
        {
            var installation = await installationRepository.GetById(id);

            if (installation == null)
                return NotFound();

            return Ok(installation);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<InstalationDTO>>> GetInstallationsByOfficeId([FromQuery] Guid officeId)
        {
            IEnumerable<Installation> installations = officeId == Guid.Empty ?
                await installationRepository.GetAll() :
                await installationRepository.GetByOfficeId(officeId);

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

        [HttpDelete]
        public async Task<ActionResult> Delete([FromQuery] Guid id)
        {
            try
            {
                await installationRepository.Delete(id);
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
