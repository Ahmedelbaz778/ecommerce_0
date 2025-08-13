namespace ECommerceAPI.Models
{
    public class Categories
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<Products> Products { get; set; }

    }
}
