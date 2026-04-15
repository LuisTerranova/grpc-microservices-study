using Microsoft.EntityFrameworkCore;
using stock.api.Interfaces;
using stock.api.Models;

namespace stock.api.Data.Repositories;

public class StockItemRepository(StockDbContext context) : IStockItemRepository
{
    public async Task<StockItem?> GetByIdAsync(Guid id) => await context.Items.FindAsync(id);

    public async Task<IEnumerable<StockItem>> GetAllAsync() => await context.Items.ToListAsync();

    public async Task AddAsync(StockItem item) => await context.Items.AddAsync(item);

    public void Remove(StockItem item) => context.Items.Remove(item);

    public async Task<bool> SaveChangesAsync() => await context.SaveChangesAsync() > 0;
}
