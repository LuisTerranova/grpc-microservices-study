using Grpc.Core;
using sales.contracts.Protos;
using stock.api.Interfaces;

namespace stock.api.Services;

public class StockGrpcService(IItemRepository repository) : StockService.StockServiceBase
{
    public override async Task<StockResponse> GetStock(
        GetStockRequest request,
        ServerCallContext context
    )
    {
        if (!Guid.TryParse(request.Id, out var id))
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid ID format"));

        var item = await repository.GetByIdAsync(id);
        if (item == null)
            throw new RpcException(
                new Status(StatusCode.NotFound, $"Stock item {request.Id} not found")
            );

        return new StockResponse
        {
            Id = item.Id.ToString(),
            Name = item.Name,
            Quantity = item.Quantity,
        };
    }

    public override async Task<StockResponse> UpdateStock(
        UpdateStockRequest request,
        ServerCallContext context
    )
    {
        if (!Guid.TryParse(request.Id, out var id))
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid ID format"));

        var item = await repository.GetByIdAsync(id);
        if (item == null)
            throw new RpcException(new Status(StatusCode.NotFound, "Item not found"));

        await repository.UpdateQuantityAsync(id, request.Quantity);
        await repository.SaveChangesAsync();

        return new StockResponse
        {
            Id = item.Id.ToString(),
            Name = item.Name,
            Quantity = request.Quantity,
        };
    }

    public override async Task<ReserveStockResponse> ReserveStock(
        ReserveStockRequest request,
        ServerCallContext context
    )
    {
        if (!Guid.TryParse(request.Id, out var id))
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid ID format"));

        var item = await repository.GetByIdAsync(id);
        if (item == null)
            return new ReserveStockResponse { Success = false, Message = "Item not found" };

        if (item.Quantity < request.Quantity)
            return new ReserveStockResponse
            {
                Success = false,
                Message = "Insufficient stock to complete the reservation",
            };

        await repository.UpdateQuantityAsync(id, item.Quantity - request.Quantity);
        await repository.SaveChangesAsync();

        return new ReserveStockResponse { Success = true, Message = "Stock reserved successfully" };
    }
}
