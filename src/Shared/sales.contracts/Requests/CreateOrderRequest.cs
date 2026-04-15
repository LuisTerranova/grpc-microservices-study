namespace sales.contracts.Requests;

public record OrderItemRequest(Guid ProductId, string Name, int Quantity, decimal Price);

public record CreateOrderRequest(List<OrderItemRequest> Items);
