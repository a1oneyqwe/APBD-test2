using Microsoft.EntityFrameworkCore;
using test.DTOs;
using test.Models;
using test.Services.Interfaces;

namespace test.Services.Implementations;

public class CustomerService : ICustomerService
{
    private readonly ApplicationDbContext _context;

    public CustomerService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CustomerDto> GetCustomerWithSubscriptionsAsync(int customerId)
    {
        var customer = await _context.Customers
            .Include(c => c.Subscriptions)
            .FirstOrDefaultAsync(c => c.Id == customerId);

        if (customer == null) return null;

        var customerDto = new CustomerDto
        {
            FirstName = customer.FirstName,
            LastName = customer.LastName,
            Email = customer.Email,
            Phone = customer.Phone,
            Discount = customer.Discount?.ToString() ?? "null",
            Subscriptions = customer.Subscriptions.Select(s => new SubscriptionDto
            {
                IdSubscription = s.Id,
                Name = s.Name,
                RenewalPeriod = s.RenewalPeriod,
                TotalPaidAmount = s.TotalPaidAmount
            }).ToList()
        };

        return customerDto;
    }

    public async Task AddPaymentAsync(PaymentDto paymentDto)
    {
        var subscription = await _context.Subscriptions
            .Include(s => s.Customer)
            .FirstOrDefaultAsync(s => s.Id == paymentDto.IdSubscription);

        if (subscription == null) throw new Exception("Subscription not found");

        var existingPayment = await _context.Payments
            .FirstOrDefaultAsync(p => p.SubscriptionId == paymentDto.IdSubscription &&
                                      p.PaymentDate.Month == DateTime.Now.Month &&
                                      p.PaymentDate.Year == DateTime.Now.Year);

        if (existingPayment != null) throw new Exception("Payment already made for this period");

        var discount = await _context.Discounts
            .Where(d => d.CustomerId == subscription.CustomerId &&
                        d.StartDate <= DateTime.Now &&
                        d.EndDate >= DateTime.Now)
            .SumAsync(d => d.Value);

        var amountToPay = paymentDto.Payment;

        if (discount > 0)
        {
            amountToPay -= amountToPay * (discount / 100);
        }

        var payment = new Payment
        {
            SubscriptionId = paymentDto.IdSubscription,
            Amount = amountToPay,
            PaymentDate = DateTime.Now
        };

        _context.Payments.Add(payment);
        await _context.SaveChangesAsync();
    }
}
