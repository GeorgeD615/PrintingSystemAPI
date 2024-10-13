using PrintingSystem.Db.Models;

namespace PrintingSystemAPI.Models
{
    public class InstallationCreateDTO
    {
        public string Name { get; set; }

        public int? InstallationOrderNumber { get; set; }

        public bool IsDefault { get; set; }

        public Guid PrintingDeviceId { get; set; }

        public Guid OfficeId { get; set; }
    }
}
