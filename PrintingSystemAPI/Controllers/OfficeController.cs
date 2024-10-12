using Microsoft.AspNetCore.Mvc;
using PrintingSystem.Db.Interfaces;
using PrintingSystemAPI.Models;

namespace PrintingSystemAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OfficeController : Controller
    {
        private readonly IOfficeRepository officeRepository;

        public OfficeController(IOfficeRepository officeRepository)
        {
            this.officeRepository = officeRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OfficeDTO>>> GetAllAsync()
        {
            var offices = await officeRepository.GetAllAsync();
            return Ok(offices.Select(o => new OfficeDTO() { Id = o.Id, Name = o.Name }));
        }
    }
}
