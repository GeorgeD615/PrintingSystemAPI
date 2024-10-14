using Microsoft.EntityFrameworkCore;
using PrintingSystem.Db.Interfaces;
using PrintingSystem.Db.Models;

namespace PrintingSystem.Db.Implementations
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly PrintingSystemContext dbcontext;

        public EmployeeRepository(PrintingSystemContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }

        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            return await dbcontext.Employees.AsNoTracking().ToListAsync();
        }
    }
}
