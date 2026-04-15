using payments.api.Models;

namespace payments.api.Interfaces;

public interface ITransactionRepository
{
    Task<Transaction?> GetByIdAsync(Guid id);
    Task<Transaction?> GetByOrderIdAsync(Guid orderId);
    Task CreateAsync(Transaction transaction);
    Task<bool> SaveChangesAsync();
}
