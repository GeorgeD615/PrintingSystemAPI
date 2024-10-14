using System.ComponentModel.DataAnnotations;

namespace PrintingSystemAPI.Models
{
    public class SessionCreateDTO
    {
        [Required(ErrorMessage = "Session task name is required.")]
        public string TaskName { get; set; }

        [Range(1, 255, ErrorMessage = "The device order number must be in the range of 1 to 255")]
        public int? DeviceOrderNumber { get; set; }

        [Required(ErrorMessage = "Employee id is required.")]
        public Guid EmployeeId { get; set; }

        [Required(ErrorMessage = "Number of pages id is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "The number of pages to print must be greater than 0")]
        public int NumberOfPages { get; set; }
    }
}
