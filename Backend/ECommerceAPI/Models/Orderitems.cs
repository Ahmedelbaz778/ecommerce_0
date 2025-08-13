using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerceAPI.Models
{
    public class Orderitems
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        [ForeignKey("orders")]
        public int orderid { get; set; }

        public Orders orders { get; set; }

        [ForeignKey("product")]
        public int productid { get; set; }

        public Products product { get; set; }


    }
}
