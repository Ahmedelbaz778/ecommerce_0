using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerceAPI.Models
{
    public class Payments
    {
        public int Id { get; set; }
        public string tMethod { get; set; }
        public string status { get; set; }
        public DateTime PaidAt { get; set; }
        [ForeignKey("Orders")]
        public int orderid {  get; set; }

        public Orders Orders { get; set; }
    }
}
