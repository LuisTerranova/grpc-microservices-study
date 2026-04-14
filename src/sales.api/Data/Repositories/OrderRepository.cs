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

    public async Task CreateAsync(Order order)
    {
        await context.Set<Order>().AddAsync(order);
    }

    public async Task UpdateStatusAsync(Guid id, OrderStatus status)
    {
        var order = await context.Set<Order>().FindAsync(id);
        if (order != null)
        {
            order.Status = status;
        }
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await context.SaveChangesAsync() >= 0;
    }
}
