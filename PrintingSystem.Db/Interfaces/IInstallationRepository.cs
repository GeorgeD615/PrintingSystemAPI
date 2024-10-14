using PrintingSystem.Db.Models;

namespace PrintingSystem.Db.Interfaces
{
    public interface IInstallationRepository
    {
        Task<Guid> CreateAsync(Installation installation);
        Task<Installation?> GetByIdAsync(Guid id);
        Task<IEnumerable<Installation>> GetAllAsync();
        Task<IEnumerable<Installation>> GetByOfficeIdAsync(Guid id);
        Task DeleteAsync(Guid id);
    }
}