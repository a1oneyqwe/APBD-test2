namespace test.Models;

public class Payment
{
    public int Id { get; set; }
    public int SubscriptionId { get; set; }
    public Subscription Subscription { get; set; }
    public DateTime PaymentDate { get; set; }
    public decimal Amount { get; set; }
}
