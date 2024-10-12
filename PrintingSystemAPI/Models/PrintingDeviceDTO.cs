using PrintingSystem.Db.Models;

namespace PrintingSystemAPI.Models
{
    public class PrintingDeviceDTO
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string ConnectionType { get; set; }
    }
}
