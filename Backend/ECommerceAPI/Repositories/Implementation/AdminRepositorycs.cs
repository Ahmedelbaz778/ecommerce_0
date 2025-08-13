using ECommerceAPI.Models;
using ECommerceAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI.Repositories.Implementations
{
    public class AdminRepository : IAdminRepository
    {
        private readonly Econtext _context;

        public AdminRepository(Econtext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Admins>> GetAllAsync()
        {
            return await _context.Admins.ToListAsync();
        }

        public async Task<Admins?> GetByIdAsync(int id)
        {
            return await _context.Admins.FindAsync(id);
        }

        public async Task AddAsync(Admins admin)
        {
            await _context.Admins.AddAsync(admin);
        }

        public void Update(Admins admin)
        {
            _context.Entry(admin).State = EntityState.Modified;
        }

        public void Delete(Admins admin)
        {
            _context.Admins.Remove(admin);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
