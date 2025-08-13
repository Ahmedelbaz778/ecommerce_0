using Microsoft.EntityFrameworkCore;



namespace ECommerceAPI.Models
{
    public class Econtext:DbContext
    {
        public DbSet<Users> Users { get; set; }
        public DbSet<Orderitems> Orderitems { get; set; }
        public DbSet<Orders> Orders { get; set; }
        public DbSet<Products> Products { get; set; }
        public DbSet<Wishlists> Wishlists { get; set; }
        public DbSet<ShippingAddresses> ShippingAddresses { get; set; }
        public DbSet<Reviews> Reviews { get; set; }
        public DbSet<Payments> Payments { get; set; }
        public DbSet<Categories> Categories { get; set; }
        public DbSet<Coupons> Coupons { get; set; }
        public DbSet<Admins> Admins { get; set; }
        public DbSet<Subscribers> Subscribers { get; set; }
        public DbSet<Brands> Brands { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Blogposts> Blogposts { get; set; }

        public Econtext(DbContextOptions<Econtext> options) : base(options)
        {


        }


    }
}
