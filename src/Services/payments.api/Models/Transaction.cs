namespace payments.api.Models;

public class Transaction
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid OrderId { get; set; }
    public decimal Amount { get; set; }
    public DateTime TransactionDate { get; set; } = DateTime.UtcNow;
    public TransactionStatus Status { get; set; } = TransactionStatus.Pending;
    public string PaymentMethod { get; set; } = "CreditCard";
}

public enum TransactionStatus
{
    Pending,
    Approved,
    Declined,
}
