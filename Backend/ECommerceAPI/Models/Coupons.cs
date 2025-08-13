namespace ECommerceAPI.Models
{
    public class Coupons
    {
        public int Id { get; set; }
        public string code { get; set; }
        public decimal discount { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
