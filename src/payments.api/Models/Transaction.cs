namespace payments.api.Models;

public class Transaction
{
    public Guid Id { get; set; }
    public Guid OrderId { get; set; }
    public decimal Amount { get; set; }
    public DateTime TransactionDate { get; set; }
    public TransactionStatus Status { get; set; }
    public string PaymentMethod { get; set; } = "CreditCard";
}

public enum TransactionStatus
{
    Pending,
    Approved,
    Declined,
}
