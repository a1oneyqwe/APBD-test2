namespace test.Models;

public class Subscription
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int RenewalPeriod { get; set; }
    public decimal TotalPaidAmount { get; set; }
    public int CustomerId { get; set; }
    public Customer Customer { get; set; }
}
