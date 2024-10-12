using Microsoft.EntityFrameworkCore;
using PrintingSystem.Db.Interfaces;
using PrintingSystem.Db.Models;

namespace PrintingSystem.Db.Implementations
{
    public class PrintingDeviceRepository : IPrintingDeviceRepository
    {
        private readonly PrintingSystemContext dbcontext;

        public PrintingDeviceRepository(PrintingSystemContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }

        public async Task<IEnumerable<PrintingDevice>> GetAllAsync()
        {
            return await dbcontext.PrintingDevices.Include(pd => pd.ConnectionType).ToListAsync();
        }

        public async Task<IEnumerable<PrintingDevice>> GetByConnectionTypeAsync(string connectionType)
        {
            return await dbcontext.PrintingDevices
                .Include(pd => pd.ConnectionType)
                .Where(pd => pd.ConnectionType!.Name.ToLower() == connectionType)
                .ToListAsync();
        }
    }
}
