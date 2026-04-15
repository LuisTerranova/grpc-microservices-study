using System.ComponentModel.DataAnnotations;

namespace stock.api.Models;

public class StockItem
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public int Quantity { get; set; }

    [Timestamp]
    public byte[] RowVersion { get; set; } = null!;
}
