using ECommerceAPI.Models;

namespace ECommerceAPI.Repositories.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Products>> GetAllAsync();
        Task<Products?> GetByIdAsync(int id);
        Task<IEnumerable<Products>> GetTrendingAsync();
        Task AddAsync(Products product);
        void Update(Products product);
        void Delete(Products product);
        Task<bool> SaveChangesAsync();
    }
}
