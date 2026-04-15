using Grpc.Core;
using sales.api.Interfaces;
using sales.api.Models;
using sales.contracts.Protos;
using sales.contracts.Requests;
using sales.Contracts.Responses;

namespace sales.api.Services;

public class SalesService(
    IOrderRepository orderRepository,
    StockService.StockServiceClient stockClient
)
{
    public async Task<TResponse<Order>> CreateOrder(CreateOrderRequest request)
    {
        var newOrder = new Order
        {
            TotalAmount = request.Items.Sum(i => i.Price * i.Quantity),
            Items = request.Items.Select(i => new Item { Name = i.Name, Price = i.Price }).ToList(),
        };

        foreach (var item in request.Items)
        {
            var reserveRequest = new ReserveStockRequest
            {
                Id = item.ProductId.ToString(),
                Quantity = item.Quantity,
            };

            try
            {
                var stockResponse = await stockClient.ReserveStockAsync(reserveRequest);
                if (!stockResponse.Success)
                {
                    return new TResponse<Order>(
                        null,
                        $"Failed to reserve item {item.Name} ({item.ProductId}): {stockResponse.Message}",
                        400
                    );
                }
            }
            catch (RpcException ex)
            {
                return new TResponse<Order>(
                    null,
                    $"Stock service is unavailable: {ex.Status.Detail}",
                    503
                );
            }
        }

        await orderRepository.AddAsync(newOrder);
        var result = await orderRepository.SaveChangesAsync();

        if (!result)
        {
            return new TResponse<Order>(null, "Failed to save the order record", 500);
        }

        // TODO: Implement event publishing to Payments API

        return new TResponse<Order>(newOrder, "Order created successfully", 201);
    }
}
