
using EVRP_Web.Data;
using EVRP_Web.Models;
using Microsoft.EntityFrameworkCore;

namespace EVRP_Web.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ApplicationDbContext _db;

        public CustomerRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<List<Customer>> GetAllAsync()
        {
            return await _db.Customers
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Customer?> GetByIdAsync(int id)
        {
            return await _db.Customers
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
