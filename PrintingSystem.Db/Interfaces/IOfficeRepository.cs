using PrintingSystem.Db.Models;

namespace PrintingSystem.Db.Interfaces
{
    public interface IOfficeRepository
    {
        Task<IEnumerable<Office>> GetAllAsync();
    }
}