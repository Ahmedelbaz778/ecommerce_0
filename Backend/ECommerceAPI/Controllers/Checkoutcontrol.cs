using ECommerceAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class CheckoutController : ControllerBase
{
    private readonly Econtext _context;

    public CheckoutController(Econtext context)
    {
        _context = context;
    }

    [HttpPost("placeorder")]
    public async Task<IActionResult> PlaceOrder([FromBody] CheckoutModel model)
    {
        if (model.Items == null || model.Items.Count == 0)
            return BadRequest("No items in the order.");

       
        var order = new Orders
        {
            totalAmount = model.TotalAmount,
            Status = "Pending",
            CreatedAt = DateTime.UtcNow,
            userid = model.UserId,
            ordersitems = new List<Orderitems>()
        };

        foreach (var item in model.Items)
        {
            var orderItem = new Orderitems
            {
                productid = item.ProductId,
                Quantity = item.Quantity,
                Price = item.Price
            };
            order.ordersitems.Add(orderItem);
        }

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

       
        var shippingAddress = new ShippingAddresses
        {
            Address = model.ShippingAddress.Address,
            City = model.ShippingAddress.City,
            Country = model.ShippingAddress.Country,
            Zipcode = model.ShippingAddress.Zipcode,
            userid = model.UserId
        };

        _context.ShippingAddresses.Add(shippingAddress);
        await _context.SaveChangesAsync();

        //Payment Step for later u baz

        return Ok(new { Message = "Order placed successfully", OrderId = order.Id });
    }

  

    public class CheckoutModel
    {
        public int UserId { get; set; }
        public decimal TotalAmount { get; set; }
        public List<CheckoutItem> Items { get; set; }
        public ShippingAddressModel ShippingAddress { get; set; }
        public string PaymentMethod { get; set; } // عشان لو فكرت اضيف قدام
    }

    public class CheckoutItem
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }

    public class ShippingAddressModel
    {
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Zipcode { get; set; }
    }
}
