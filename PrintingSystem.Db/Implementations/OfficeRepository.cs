using Microsoft.EntityFrameworkCore;
using PrintingSystem.Db.Interfaces;
using PrintingSystem.Db.Models;

namespace PrintingSystem.Db.Implementations
{
    public class OfficeRepository : IOfficeRepository
    {
        private readonly PrintingSystemContext dbcontext;

        public OfficeRepository(PrintingSystemContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }

        public async Task<IEnumerable<Office>> GetAllAsync()
        {
            return await dbcontext.Offices.ToListAsync();
        }
    }
}
