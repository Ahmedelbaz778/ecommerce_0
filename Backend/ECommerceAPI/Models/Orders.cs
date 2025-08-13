using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerceAPI.Models
{
    public class Orders
    {
        public int Id { get; set; }
        public decimal totalAmount {  get; set; }
        public String Status { get; set; }
        public DateTime CreatedAt { get; set; }

        [ForeignKey("users")]
        public int userid { get; set; }

        public Users users { get; set; }
        public List<Payments>payments { get; set; }
        public List<Orderitems> ordersitems { get; set; }
        public string ShippingAddress { get; set; } 
        public string PaymentMethod { get; set; }

    }
}
