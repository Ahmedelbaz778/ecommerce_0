using ECommerceAPI.Models;
using ECommerceAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI.Repositories.Implementations
{
    public class OrderRepository : IOrderRepository
    {
        private readonly Econtext _context;

        public OrderRepository(Econtext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Orders>> GetAllAsync()
        {
            return await _context.Orders
                .Include(o => o.users)
                .Include(o => o.payments)
                .Include(o => o.ordersitems)
                .ToListAsync();
        }

        public async Task<Orders?> GetByIdAsync(int id)
        {
            return await _context.Orders
                .Include(o => o.users)
                .Include(o => o.payments)
                .Include(o => o.ordersitems)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<Orders> CreateAsync(Orders order)
        {
            order.CreatedAt = DateTime.Now;
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<bool> UpdateAsync(Orders order)
        {
            _context.Entry(order).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Orders.Any(e => e.Id == order.Id))
                    return false;
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
                return false;

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
