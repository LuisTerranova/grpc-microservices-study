using sales.api.Models;

namespace sales.api.Interfaces;

public interface IOrderRepository
{
    Task<Order?> GetByIdAsync(Guid id);
    Task<IEnumerable<Order>> GetAllAsync();
    Task CreateAsync(Order order);
    Task UpdateStatusAsync(Guid id, OrderStatus status);
    Task<bool> SaveChangesAsync();
}
