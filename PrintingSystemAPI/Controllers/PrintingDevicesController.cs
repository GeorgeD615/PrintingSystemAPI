using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PrintingSystem.Db.Interfaces;
using PrintingSystem.Db.Models;
using PrintingSystemAPI.Models;

namespace PrintingSystemAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PrintingDevicesController : ControllerBase
    {
        private readonly IPrintingDeviceRepository printingDeviceRepository;

        public PrintingDevicesController(IPrintingDeviceRepository printingDeviceRepository)
        {
            this.printingDeviceRepository = printingDeviceRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PrintingDeviceDTO>>> GetAllAsync([FromQuery] string? connectionType = null)
        {
            IEnumerable<PrintingDevice> printingDevices;
            connectionType = connectionType?.Trim().ToLower();

            printingDevices = connectionType.IsNullOrEmpty() ?
                await printingDeviceRepository.GetAllAsync() :
                await printingDeviceRepository.GetByConnectionTypeAsync(connectionType);

            return Ok(printingDevices.Select(pd => new PrintingDeviceDTO()
            {
                Id = pd.Id,
                Name = pd.Name,
                ConnectionType = pd.ConnectionType!.Name
            }));
        }
    }
}
