namespace PrintingSystemAPI.Models
{
    public class SessionCreateDTO
    {
        public string TaskName { get; set; }

        public int? DeviceOrderNumber { get; set; }

        public Guid EmployeeId { get; set; }

        public int NumberOfPages { get; set; }
    }
}
