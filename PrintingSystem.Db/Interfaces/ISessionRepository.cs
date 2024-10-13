using PrintingSystem.Db.Models;

namespace PrintingSystem.Db.Interfaces
{
    public interface ISessionRepository
    {
        Task<bool> Create(Session session, int? installationOrderNumber);
    }
}