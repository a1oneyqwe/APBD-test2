using test.DTOs;

namespace test.Services.Interfaces;

public interface ICustomerService
{
    Task<CustomerDto> GetCustomerWithSubscriptionsAsync(int customerId);
    Task AddPaymentAsync(PaymentDto paymentDto);
}