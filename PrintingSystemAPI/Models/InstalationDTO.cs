namespace PrintingSystemAPI.Models
{
    public class InstalationDTO
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public int? InstallationOrderNumber { get; set; }

        public bool IsDefault { get; set; }

        public Guid PrintingDeviceId { get; set; }

        public Guid OfficeId { get; set; }
    }
}
