using Grpc.Core;
using sales.contracts.Protos;
using sales.core.Extensions;
using Protos = sales.contracts.Protos;

namespace stock.api.Services;

public class StockGrpcService(StockService stockService) : Protos.StockService.StockServiceBase
{
    public override async Task<ReserveStockResponse> ReserveStock(
        ReserveStockRequest request,
        ServerCallContext context
    )
    {
        if (!Guid.TryParse(request.Id, out var id))
        {
            throw new RpcException(
                new Status(StatusCode.InvalidArgument, "Invalid stock item ID format")
            );
        }

        var result = await stockService.ReserveStockAsync(id, request.Quantity);

        if (!result.IsSuccess)
        {
            throw result.ToRpcException();
        }

        return new ReserveStockResponse { Success = true, Message = result.Message };
    }
}
