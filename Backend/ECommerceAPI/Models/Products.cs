using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerceAPI.Models
{
    public class Products
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsTrending { get; set; }
        [ForeignKey("Brands")]
        public int Brandid { get; set; }

        public Brands Brands { get; set; }

        [ForeignKey("categories")]
        public int categoriesid { get; set; }

        public Categories categories { get; set; }
        public List<Reviews> Reviews { get; set; }
        public List<Wishlists> Wishlists { get; set; }
        public List<Orderitems> orderitems { get; set; }



    }
}
