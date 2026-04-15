namespace sales.api.Models;

public class Order
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    public decimal TotalAmount { get; set; }
    public OrderStatus Status { get; set; } = OrderStatus.Pending;
    public List<Item> Items { get; set; } = new();
}

public enum OrderStatus
{
    Pending,
    Completed,
    Cancelled,
}
