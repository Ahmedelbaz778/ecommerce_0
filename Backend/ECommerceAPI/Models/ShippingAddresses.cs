using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerceAPI.Models
{
    public class ShippingAddresses
    {
        public int Id { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Zipcode { get; set; }
        [ForeignKey("users")]
        public int userid { get; set; }

        public Users users { get; set; }
    }
}
