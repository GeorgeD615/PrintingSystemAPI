using PrintingSystem.Db.Models;

namespace PrintingSystem.Db.Interfaces
{
    public interface IInstallationRepository
    {
        Task<Guid> CreateAsync(Installation installation);
        Task<Installation> GetById(Guid id);
        Task<IEnumerable<Installation>> GetAll();
        Task<IEnumerable<Installation>> GetByOfficeId(Guid id);
        Task Delete(Guid id);
    }
}