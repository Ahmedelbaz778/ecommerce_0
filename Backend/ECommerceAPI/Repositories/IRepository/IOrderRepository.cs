using ECommerceAPI.Models;

namespace ECommerceAPI.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Orders>> GetAllAsync();
        Task<Orders?> GetByIdAsync(int id);
        Task<Orders> CreateAsync(Orders order);
        Task<bool> UpdateAsync(Orders order);
        Task<bool> DeleteAsync(int id);
    }
}
