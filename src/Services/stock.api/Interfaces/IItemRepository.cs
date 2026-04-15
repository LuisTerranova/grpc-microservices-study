using stock.api.Models;

namespace stock.api.Interfaces;

public interface IStockItemRepository
{
    Task<StockItem?> GetByIdAsync(Guid id);
    Task<IEnumerable<StockItem>> GetAllAsync();
    Task AddAsync(StockItem item);
    void Remove(StockItem item);
    Task<bool> SaveChangesAsync();
}
