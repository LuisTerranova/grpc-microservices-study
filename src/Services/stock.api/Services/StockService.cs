using sales.Contracts.Responses;
using stock.api.Interfaces;
using stock.api.Models;

namespace stock.api.Services;

public class StockService(IStockItemRepository repository)
{
    public async Task<TResponse<StockItem>> GetByIdAsync(Guid id)
    {
        var item = await repository.GetByIdAsync(id);

        if (item is null)
            return new TResponse<StockItem>(null, "Stock item not found", 404);

        return new TResponse<StockItem>(item, "Stock item retrieved successfully", 200);
    }

    public async Task<TResponse<IEnumerable<StockItem>>> GetAllAsync()
    {
        var items = await repository.GetAllAsync();
        return new TResponse<IEnumerable<StockItem>>(
            items,
            "Stock items list retrieved successfully"
        );
    }

    public async Task<TResponse<StockItem>> CreateAsync(StockItem item)
    {
        await repository.AddAsync(item);
        var result = await repository.SaveChangesAsync();

        if (!result)
            return new TResponse<StockItem>(null, "Could not create stock item", 400);

        return new TResponse<StockItem>(item, "Stock item created successfully", 201);
    }

    public async Task<TResponse<bool>> DeleteAsync(Guid id)
    {
        var item = await repository.GetByIdAsync(id);

        if (item is null)
            return new TResponse<bool>(false, "Stock item not found", 404);

        repository.Remove(item);
        var result = await repository.SaveChangesAsync();

        if (!result)
            return new TResponse<bool>(false, "Could not remove stock item", 400);

        return new TResponse<bool>(true, "Stock item removed successfully");
    }

    public async Task<TResponse<StockItem>> ReserveStockAsync(Guid id, int quantity)
    {
        var item = await repository.GetByIdAsync(id);

        if (item is null)
            return new TResponse<StockItem>(null, "Stock item not found", 404);

        if (item.Quantity < quantity)
            return new TResponse<StockItem>(null, "Insufficient stock", 400);

        item.Quantity -= quantity;

        var result = await repository.SaveChangesAsync();

        if (!result)
            return new TResponse<StockItem>(null, "Could not reserve stock", 400);

        return new TResponse<StockItem>(item, "Stock reserved successfully");
    }
}
