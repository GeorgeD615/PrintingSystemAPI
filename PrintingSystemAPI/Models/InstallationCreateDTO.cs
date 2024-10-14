using System.ComponentModel.DataAnnotations;

namespace PrintingSystemAPI.Models
{
    public class InstallationCreateDTO
    {
        [Required(ErrorMessage = "Installation name is required.")]
        public string Name { get; set; }

        [Range(1, 255, ErrorMessage = "The device order number must be in the range of 1 to 255")]
        public int? DeviceOrderNumber { get; set; }

        public bool IsDefault { get; set; }

        [Required(ErrorMessage = "Printing device id is required.")]
        public Guid PrintingDeviceId { get; set; }

        [Required(ErrorMessage = "Office id is required.")]
        public Guid OfficeId { get; set; }
    }
}
