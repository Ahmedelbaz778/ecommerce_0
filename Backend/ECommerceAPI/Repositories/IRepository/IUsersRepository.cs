using ECommerceAPI.Models;

namespace ECommerceAPI.Repositories.Interfaces
{
    public interface IUsersRepository
    {
        Task<IEnumerable<Users>> GetAllAsync();
        Task<Users?> GetByIdAsync(int id);
        Task<Users> AddAsync(Users user);
        Task<bool> UpdateAsync(Users user);
        Task<bool> DeleteAsync(int id);
    }
}