namespace sales.api.Models;

public class CreateOrderRequest
{
    public List<OrderItemRequest> Items { get; set; } = new();
}

public class OrderItemRequest
{
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
}
