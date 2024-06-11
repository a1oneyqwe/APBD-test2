using Microsoft.AspNetCore.Mvc;
using test.DTOs;
using test.Services.Interfaces;

namespace test.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomerController : ControllerBase
{
    private readonly ICustomerService _customerService;

    public CustomerController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCustomer(int id)
    {
        var customer = await _customerService.GetCustomerWithSubscriptionsAsync(id);
        if (customer == null) return NotFound();
        return Ok(customer);
    }

    [HttpPost("payment")]
    public async Task<IActionResult> AddPayment([FromBody] PaymentDto paymentDto)
    {
        try
        {
            await _customerService.AddPaymentAsync(paymentDto);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
