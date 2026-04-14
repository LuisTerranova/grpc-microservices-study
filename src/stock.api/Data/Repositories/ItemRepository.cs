using Microsoft.EntityFrameworkCore;
using stock.api.Data;
using stock.api.Models;
using stock.api.Interfaces;

namespace stock.api.Data.Repositories;

public class ItemRepository(StockDbContext context) : IItemRepository
{
    public async Task<Item?> GetByIdAsync(Guid id)
    {
        return await context.Items.FindAsync(id);
    }

    public async Task<IEnumerable<Item>> GetAllAsync()
    {
        return await context.Items.ToListAsync();
    }

    public async Task UpdateQuantityAsync(Guid id, int quantity)
    {
        var item = await context.Items.FindAsync(id);
        if (item != null)
        {
            item.Quantity = quantity;
        }
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await context.SaveChangesAsync() >= 0;
    }
}
