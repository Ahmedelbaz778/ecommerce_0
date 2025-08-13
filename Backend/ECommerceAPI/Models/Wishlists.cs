using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerceAPI.Models
{
    public class Wishlists
    {
        public int Id { get; set; }
        [ForeignKey("users")]
        public int userid { get; set; }

        public Users users { get; set; }

        [ForeignKey("product")]
        public int productid { get; set; }

        public Products product { get; set; }


    }
}
