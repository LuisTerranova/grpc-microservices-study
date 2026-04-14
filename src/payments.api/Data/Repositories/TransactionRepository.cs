using Microsoft.EntityFrameworkCore;
using payments.api.Data;
using payments.api.Models;
using payments.api.Interfaces;

namespace payments.api.Data.Repositories;

public class TransactionRepository(PaymentsDbContext context) : ITransactionRepository
{
    public async Task<Transaction?> GetByIdAsync(Guid id)
    {
        return await context.Transactions.FindAsync(id);
    }

    public async Task<Transaction?> GetByOrderIdAsync(Guid orderId)
    {
        return await context.Transactions.FirstOrDefaultAsync(t => t.OrderId == orderId);
    }

    public async Task CreateAsync(Transaction transaction)
    {
        await context.Transactions.AddAsync(transaction);
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await context.SaveChangesAsync() >= 0;
    }
}
