namespace test.Models;

public class Discount
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public decimal Value { get; set; } // expressed as a percentage
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public Customer Customer { get; set; }
}
