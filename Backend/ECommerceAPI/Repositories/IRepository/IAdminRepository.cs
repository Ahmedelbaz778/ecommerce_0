using ECommerceAPI.Models;

namespace ECommerceAPI.Repositories.Interfaces
{
    public interface IAdminRepository
    {
        Task<IEnumerable<Admins>> GetAllAsync();
        Task<Admins?> GetByIdAsync(int id);
        Task AddAsync(Admins admin);
        void Update(Admins admin);
        void Delete(Admins admin);
        Task<bool> SaveChangesAsync();
    }
}
