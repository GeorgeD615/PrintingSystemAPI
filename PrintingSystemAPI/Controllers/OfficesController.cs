using Microsoft.AspNetCore.Mvc;
using PrintingSystem.Db.Interfaces;
using PrintingSystemAPI.Models;

namespace PrintingSystemAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OfficesController : ControllerBase
    {
        private readonly IOfficeRepository officeRepository;

        public OfficesController(IOfficeRepository officeRepository)
        {
            this.officeRepository = officeRepository;
        }

        /// <summary>
        /// Получает список всех офисов.
        /// </summary>
        /// <returns>Список офисов.</returns>
        /// <response code="200">Возвращает список офисов.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<OfficeDTO>>> GetAllAsync()
        {
            var offices = await officeRepository.GetAllAsync();
            return Ok(offices.Select(o => new OfficeDTO() { 
                Id = o.Id, 
                Name = o.Name 
            }));
        }
    }
}
