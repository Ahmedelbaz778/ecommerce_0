namespace ECommerceAPI.Models
{
    public class Brands
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime createdAt { get; set; }
        public string? LogoUrl { get; set; }
        public List<Products> Products { get; set; }
    }
}
