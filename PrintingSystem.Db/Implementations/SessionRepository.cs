using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PrintingSystem.Db.Interfaces;
using PrintingSystem.Db.Models;
using System.Text;

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

        public async Task<bool> CreateAsync(Session session, int? installationOrderNumber, bool simulateDelay = true)
        {
            if (string.IsNullOrWhiteSpace(session.TaskName))
                throw new ArgumentException("Print task name is required.");

            if (session.NumberOfPages <= 0)
                throw new ArgumentException("Page count must be a positive integer.");

            var employee = await dbcontext.Employees
                .Include(e => e.Office)
                .ThenInclude(o => o.Installations)
                .AsNoTracking()
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
                    .AsNoTracking()
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

            if (simulateDelay)
            {
                var delay = random.Next(1000, 4000);
                await Task.Delay(delay);
            }

            bool isSuccess = random.Next(0, 2) == 1;

            session.StatusId = isSuccess ?
                (await dbcontext.SeccionStatuses.FirstOrDefaultAsync(s => s.Name.ToLower() == "успех")).Id :
                (await dbcontext.SeccionStatuses.FirstOrDefaultAsync(s => s.Name.ToLower() == "неудача")).Id;

            await dbcontext.Sessions.AddAsync(session);
            await dbcontext.SaveChangesAsync();

            return isSuccess;
        }

        public async Task<int> ProcessSessionsFromCsvAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File is empty or missing.");

            var result = 0;

            using (var streamReader = new StreamReader(file.OpenReadStream(), Encoding.UTF8))
            {
                string line;
                int lineNumber = 0;

                while ((line = await streamReader.ReadLineAsync()) != null)
                {
                    lineNumber++;

                    if (lineNumber > 100)
                        break;

                    var fields = line.Split(';');

                    if (fields.Length < 4 || fields.Any(string.IsNullOrWhiteSpace))
                        continue;

                    try
                    {
                        var newSession = new Session
                        {
                            TaskName = fields[0],
                            EmployeeId = Guid.Parse(fields[1]),
                            NumberOfPages = int.Parse(fields[3])
                        };
                        int? installationOrderNumber = int.Parse(fields[2]);

                        await CreateAsync(newSession, installationOrderNumber, false);
                        ++result;
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }
            }

            if (result == 0)
                throw new InvalidOperationException("Incorrect file format. Cannot retrieve data from the file.");

            return result;
        }
    }
}
