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

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] InstallationCreateDTO installationDto)
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

                return Ok( new { id = createdInstallationId } );
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
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var installation = await installationRepository.GetByIdAsync(id);

            if (installation == null)
                return NotFound();

            return Ok(installation);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<InstalationDTO>>> GetAllByOfficeIdAsync([FromQuery] Guid? officeId)
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

        [HttpDelete("{id}")]
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
