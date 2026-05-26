
using EVRP_Web.Models;

namespace EVRP_Web.Repositories
{
    public interface ICustomerRepository
    {
        Task<List<Customer>> GetAllAsync();
        Task<Customer?> GetByIdAsync(int id);
        Task SaveAsync();
    }
}
