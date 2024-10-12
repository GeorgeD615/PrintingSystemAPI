using PrintingSystem.Db.Models;

namespace PrintingSystem.Db.Interfaces
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<Employee>> GetAllAsync();
    }
}