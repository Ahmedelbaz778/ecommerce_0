using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI.Models
{
    public class Users
    {
        
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }

        public string Phone { get; set; }

        public List<Orders>? orders { get; set; }
        public List<ShippingAddresses>? shippingAddresses { get; set; }
        public List<Reviews>? reviews { get; set; } 
        public List<Wishlists>? wishlists { get; set; }
        
    


    }
}
