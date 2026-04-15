using sales.api.Models;

namespace sales.api.Interfaces;

public interface IOrderRepository
{
    Task<Order?> GetByIdAsync(Guid id);
    Task<IEnumerable<Order>> GetAllAsync();
    Task AddAsync(Order order);
    void Remove(Order order);
    Task<bool> SaveChangesAsync();
}
