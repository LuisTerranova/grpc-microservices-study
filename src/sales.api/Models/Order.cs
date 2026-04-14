namespace sales.api.Models;

public class Order
{
    public Guid Id { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal TotalAmount { get; set; }
    public OrderStatus Status { get; set; }
    public List<Item> Items { get; set; } = new();
}

public enum OrderStatus
{
    Pending,
    Completed,
    Cancelled,
}
