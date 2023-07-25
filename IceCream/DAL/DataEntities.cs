using IceCream.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity; 
using System.Linq;
using System.Web; 
  
namespace IceCream.DAL
{ 
    public class DataEntities : DbContext
    { 
        public DataEntities() : base("name=DataEntities") { }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<ConfigSite> ConfigSites { get; set; }
        public DbSet<Banner> Banners { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<ArticleCategory> ArticleCategories { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        
        //
        public DbSet<Cart> Carts { get; set; }  
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

    }
}