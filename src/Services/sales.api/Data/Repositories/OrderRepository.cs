using Microsoft.EntityFrameworkCore;
using sales.api.Interfaces;
using sales.api.Data;
using sales.api.Models;

namespace sales.api.Data.Repositories;

public class OrderRepository(SalesDbContext context) : IOrderRepository
{
    public async Task<Order?> GetByIdAsync(Guid id)
    {
        return await context
            .Set<Order>()
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task<IEnumerable<Order>> GetAllAsync()
    {
        return await context.Set<Order>().Include(o => o.Items).ToListAsync();
    }

    public async Task AddAsync(Order order)
    {
        await context.Set<Order>().AddAsync(order);
    }

    public void Remove(Order order)
    {
        context.Set<Order>().Remove(order);
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await context.SaveChangesAsync() >= 0;
    }
}
