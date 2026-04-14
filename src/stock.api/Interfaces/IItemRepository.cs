using stock.api.Models;

namespace stock.api.Interfaces;

public interface IItemRepository
{
    Task<Item?> GetByIdAsync(Guid id);
    Task<IEnumerable<Item>> GetAllAsync();
    Task UpdateQuantityAsync(Guid id, int quantity);
    Task<bool> SaveChangesAsync();
}
