using EVRP_Web.Data;
using EVRP_Web.Models;
using Microsoft.EntityFrameworkCore;

namespace EVRP_Web.Services
{
    public class CustomerService
    {
        private readonly ApplicationDbContext _db;

        public CustomerService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<List<Customer>> GetAllAsync()
        {
            return await _db.Customers
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<Customer>> GetDeliveredAsync()
        {
            return await _db.Customers
                .Where(x => x.IsDelivered)
                .ToListAsync();
        }

        public async Task ResetDeliveredAsync()
        {
            var delivered = await _db.Customers
                .Where(x => x.IsDelivered)
                .ToListAsync();

            foreach (var item in delivered)
            {
                item.IsDelivered = false;
                item.ActualDeliveryTime = null;
            }

            await _db.SaveChangesAsync();
        }

        public async Task<Customer?> FindAsync(int id)
        {
            return await _db.Customers
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task UpdateAsync(Customer customer)
        {
            _db.Customers.Update(customer);

            await _db.SaveChangesAsync();
        }
    }
}
