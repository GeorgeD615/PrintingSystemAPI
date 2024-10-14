using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using PrintingSystem.Db.Interfaces;
using PrintingSystem.Db.Models;

namespace PrintingSystem.Db.Implementations
{
    public class InstallationRepository : IInstallationRepository
    {
        private readonly PrintingSystemContext dbcontext;
        private readonly IMemoryCache memoryCache;
        private readonly TimeSpan cacheDuration = TimeSpan.FromMinutes(105);
        private const string CacheKey = "installations_cache";
        private int MaxInstallationOrderNumber { get; } = 255;

        public InstallationRepository(PrintingSystemContext dbcontext, IMemoryCache memoryCache)
        {
            this.dbcontext = dbcontext;
            this.memoryCache = memoryCache;
        }

        public async Task<Guid> CreateAsync(Installation installation)
        {
            var office = await dbcontext.Offices.FindAsync(installation.OfficeId);

            if (office == null)
                throw new ArgumentException("Office does not exist.");

            var printingDevice = await dbcontext.PrintingDevices.FindAsync(installation.PrintingDeviceId);

            if (printingDevice == null)
                throw new ArgumentException("Printing device does not exist.");

            var installationsInOffice = dbcontext.Installations
                .AsNoTracking()
                .Where(inst => inst.OfficeId == installation.OfficeId);

            if (installation.InstallationOrderNumber != null)
            {
                bool orderNumberExists = await installationsInOffice
                    .AnyAsync(inst => inst.InstallationOrderNumber == installation.InstallationOrderNumber);

                if (orderNumberExists)
                    throw new InvalidOperationException("Installation with the specified order number already exists.");
            }
            else
            {
                var maxOrderNumber = await installationsInOffice
                    .MaxAsync(inst => inst.InstallationOrderNumber) ?? 0;

                if (maxOrderNumber >= MaxInstallationOrderNumber)
                    throw new InvalidOperationException("Maximum installation order number reached for this office.");

                installation.InstallationOrderNumber = maxOrderNumber + 1;
            }

            var defaultInstallationExists = await installationsInOffice
                .AnyAsync(inst => inst.IsDefault);

            if (installation.IsDefault)
            {
                if (defaultInstallationExists)
                    throw new InvalidOperationException("There is already a default installation for this office.");
            }
            else if (!installationsInOffice.Any() && !installation.IsDefault)
                throw new InvalidOperationException("The first installation in the office must be set as default.");

            await dbcontext.Installations.AddAsync(installation);

            await dbcontext.SaveChangesAsync();

            await UpdateCache();

            return installation.Id;
        }

        public async Task<Installation?> GetByIdAsync(Guid id)
        {
            var installations = await GetInstallationsFromCache();
            return installations!.FirstOrDefault(inst => inst.Id == id);
        }

        public async Task<IEnumerable<Installation>> GetByOfficeIdAsync(Guid id)
        {
            var installations = await GetInstallationsFromCache();

            var office = await dbcontext.Offices.FindAsync(id);

            if (office == null)
                throw new KeyNotFoundException("Office not found.");

            return installations!.Where(inst => inst.OfficeId == office.Id);
        }

        public async Task<IEnumerable<Installation>> GetAllAsync()
        {
            var installations = await GetInstallationsFromCache();
            return installations;
        }

        private async Task<IEnumerable<Installation>> GetInstallationsFromCache()
        {
            if (!memoryCache.TryGetValue(CacheKey, out IEnumerable<Installation> installations))
            {
                installations = await dbcontext.Installations.AsNoTracking().ToListAsync();

                var cacheOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = cacheDuration
                };

                memoryCache.Set(CacheKey, installations, cacheOptions);
            }

            return installations;
        }

        public async Task DeleteAsync(Guid id)
        {
            var installation = await dbcontext.Installations.FindAsync(id);

            if (installation == null)
                throw new KeyNotFoundException("Installation not found.");

            if (installation.IsDefault)
                throw new InvalidOperationException("Cannot delete the default installation.");

            dbcontext.Installations.Remove(installation);
            await dbcontext.SaveChangesAsync();

            await UpdateCache();
        }

        public async Task UpdateCache()
        {
            var installations = await dbcontext.Installations.AsNoTracking().ToListAsync();

            var cacheOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = cacheDuration
            };

            memoryCache.Set(CacheKey, installations, cacheOptions);
        }
    }
}
