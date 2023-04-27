using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;

namespace swaggerExp.Controllers;

[ApiController]
[Route("1.0/customers")]
public class CustomersController : ControllerBase
{
    private readonly ILogger<CustomersController> _logger;

    private static int CustomersCounter = 1;
    private static object _lock = new object();



    private static readonly List<Customer> _customers = new List<Customer>{
        new Customer(++CustomersCounter, "John"),
        new Customer(++CustomersCounter, "Mary")
    };


    public CustomersController(ILogger<CustomersController> logger)
    {
        _logger = logger;
    }



    [HttpGet()]
    public IEnumerable<Customer> Get()
    {
        return _customers.ToArray();
    }

    [HttpPost("create")]
    public IActionResult Create(CustomerCreationArgs customer)
    {
        lock (_lock)
        {
            _customers.Add(new Customer(++CustomersCounter, customer.Name));
            return Ok();
        }
        
    }

    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        var customer = _customers.FirstOrDefault(c => c.Id == id);
        if (customer == null)
        {
            return NotFound();
        }
        else
        {
            return Ok(customer);
        }
    }

}





