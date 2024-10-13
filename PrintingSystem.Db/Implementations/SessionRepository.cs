using Microsoft.EntityFrameworkCore;
using PrintingSystem.Db.Interfaces;
using PrintingSystem.Db.Models;

namespace PrintingSystem.Db.Implementations
{
    public class SessionRepository : ISessionRepository
    {
        private readonly PrintingSystemContext dbcontext;
        private readonly Random random;

        public SessionRepository(PrintingSystemContext dbcontext)
        {
            this.dbcontext = dbcontext;
            random = new Random();
        }

        public async Task<bool> Create(Session session, int? installationOrderNumber)
        {
            if (string.IsNullOrWhiteSpace(session.TaskName))
                throw new ArgumentException("Print task name is required.");

            if (session.NumberOfPages <= 0)
                throw new ArgumentException("Page count must be a positive integer.");

            var employee = await dbcontext.Employees
                .Include(e => e.Office)
                .ThenInclude(o => o.Installations)
                .FirstOrDefaultAsync(e => e.Id == session.EmployeeId);

            if (employee == null)
                throw new ArgumentException("Employee not found.");

            var office = employee.Office;
            if (office == null)
                throw new ArgumentException("Employee's office not found.");

            Installation? installation;

            if (installationOrderNumber != null)
            {
                installation = await dbcontext.Installations
                    .Where(inst => inst.OfficeId == office.Id)
                    .FirstOrDefaultAsync(inst => inst.InstallationOrderNumber == installationOrderNumber);

                if (installation == null)
                    throw new ArgumentException("Printing device with the specified order number not found in this office.");
            }
            else
            {
                installation = await dbcontext.Installations
                    .Where(inst => inst.OfficeId == office.Id)
                    .FirstOrDefaultAsync(inst => inst.IsDefault);

                if (installation == null)
                    throw new ArgumentException("No default printing device found for this office.");
            }

            session.InstallationId = installation.Id;

            var delay = random.Next(1000, 4000);
            await Task.Delay(delay);

            bool isSuccess = random.Next(0, 2) == 1;

            session.StatusId = isSuccess ?
                (await dbcontext.SeccionStatuses.FirstOrDefaultAsync(s => s.Name.ToLower() == "успех")).Id :
                (await dbcontext.SeccionStatuses.FirstOrDefaultAsync(s => s.Name.ToLower() == "неудача")).Id;

            await dbcontext.Sessions.AddAsync(session);
            await dbcontext.SaveChangesAsync();

            return isSuccess;
        }
    }
}
