using PrintingSystem.Db.Models;

namespace PrintingSystem.Db.Interfaces
{
    public interface IPrintingDeviceRepository
    {
        Task<IEnumerable<PrintingDevice>> GetAllAsync();

        Task<IEnumerable<PrintingDevice>> GetByConnectionTypeAsync(string connectionType);
    }
}