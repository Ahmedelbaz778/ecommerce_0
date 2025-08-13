using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerceAPI.Models
{
    public class Reviews
    {
        public int Id { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; }
        public int Rating { get; set; }
        [ForeignKey("users")]
        public int userid { get; set; }

        public Users users { get; set; }

        [ForeignKey("products")]
        public int productid { get; set; }

        public Products products { get; set; }

    }
}
