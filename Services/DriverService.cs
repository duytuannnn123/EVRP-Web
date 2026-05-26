using EVRP_Web.Data;
using EVRP_Web.Models;
using Microsoft.EntityFrameworkCore;

namespace EVRP_Web.Services
{
    public class DriverService
    {
        private readonly ApplicationDbContext _db;

        public DriverService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<List<Driver>> GetActiveDriversAsync()
        {
            return await _db.Drivers
                .AsNoTracking()
                .Where(x => x.IsActive)
                .ToListAsync();
        }
    }
}
