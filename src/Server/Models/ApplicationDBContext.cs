using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Zalandu.Shared;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Zalandu.Server.Models
{
    public class ApplicationDBContext : IdentityDbContext
    {
        public DbSet<Product>      Products       { get; set; }
        public DbSet<Order>        CustomerOrders { get; set; }
        public DbSet<OrderItem>    OrderItems     { get; set; }
        public DbSet<Address>      Addresses      { get; set; }
        public DbSet<Category>     Categories     { get; set; }
        public DbSet<StockProduct> StockProducts  { get; set; }

        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options):base(options){}

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //----AspNetUsers table is mapped against to model CustomerIdentity
            builder.Entity<IdentityCustomer>();

            builder.Entity<Address>().ToTable("Addresses");
            builder.Entity<Address>().HasKey((Address   address) => address.AddressId);
            builder.Entity<Address>().Property((Address address) => address.PhoneNumber).IsRequired();
            builder.Entity<Address>().Property((Address address) => address.CustomerId).IsRequired();
            builder.Entity<Address>().Property((Address address) => address.RoadName).IsRequired();
            builder.Entity<Address>().Property((Address address) => address.RoadNumber).IsRequired();
            builder.Entity<Address>().Property((Address address) => address.Block);
            builder.Entity<Address>().Property((Address address) => address.Floor);
            builder.Entity<Address>().Property((Address address) => address.Door);
            builder.Entity<Address>().Property((Address address) => address.Province);
            builder.Entity<Address>().Property((Address address) => address.Municipality);
                                                                      
            builder.Entity<Product>().ToTable("Products");
            builder.Entity<Product>().HasKey((Product   prod) => prod.ProductId);
            builder.Entity<Product>().Property((Product prod) => prod.CategoryId).IsRequired();
            builder.Entity<Product>().Property((Product prod) => prod.CategoryParent);
            builder.Entity<Product>().Property((Product prod) => prod.CategoryChild);
            builder.Entity<Product>().Property((Product prod) => prod.CategoryProduct).IsRequired();
            builder.Entity<Product>().Property((Product prod) => prod.ProductName).IsRequired();
            builder.Entity<Product>().Property((Product prod) => prod.Brand).IsRequired();
            builder.Entity<Product>().Property((Product prod) => prod.Price).IsRequired().HasColumnType("DECIMAL(5,2)"); 
            builder.Entity<Product>().Property((Product prod) => prod.Discount).IsRequired().HasColumnType("DECIMAL(5,2)"); 
            builder.Entity<Product>().Property((Product prod) => prod.Color).IsRequired();
            builder.Entity<Product>().Property((Product prod) => prod.ImageId).IsRequired();
            builder.Entity<Product>().Property((Product prod) => prod.Materials).IsRequired();
            builder.Entity<Product>().Property((Product prod) => prod.Feature).IsRequired();
            builder.Entity<Product>().Property((Product prod) => prod.SizeCut).IsRequired();
            builder.Entity<Product>().Property((Product prod) => prod.Sustainability);
            builder.Entity<Product>().Property((Product prod) => prod.Rating).IsRequired().HasColumnType("DECIMAL(5,2)");

            builder.Entity<StockProduct>().ToTable("StockProducts");
            builder.Entity<StockProduct>().HasKey((StockProduct   stock) => new { stock.ProductId, stock.Size }); 
            builder.Entity<StockProduct>().Property((StockProduct stock) => stock.ProductId).IsRequired();
            builder.Entity<StockProduct>().Property((StockProduct stock) => stock.Size).IsRequired();
            builder.Entity<StockProduct>().Property((StockProduct stock) => stock.SizePrice).HasColumnType("DECIMAL(5,2)");
            builder.Entity<StockProduct>().Property((StockProduct stock) => stock.Stock).IsRequired();

            builder.Entity<Order>().ToTable("CustomerOrders");
            builder.Entity<Order>().HasKey((Order   order) => order.OrderId);
            builder.Entity<Order>().Property((Order order) => order.CustomerId).IsRequired();           
            builder.Entity<Order>().Property((Order order) => order.StatusOrder).IsRequired();
            builder.Entity<Order>().Property((Order order) => order.DateOrder).IsRequired();
            builder.Entity<Order>().Property((Order order) => order.DeliveryCosts).IsRequired().HasColumnType("DECIMAL(5,2)"); 
            builder.Entity<Order>().Property((Order order) => order.SubtotalOrder).IsRequired().HasColumnType("DECIMAL(5,2)"); 
            builder.Entity<Order>().Property((Order order) => order.TotalOrder).IsRequired().HasColumnType("DECIMAL(5,2)");
            builder.Entity<Order>().Property((Order order) => order.OrderList).IsRequired().HasColumnName("OrderItems")
                   .HasConversion(orderItem => JsonSerializer.Serialize(orderItem, (JsonSerializerOptions)null),
                                  orderItem => JsonSerializer.Deserialize<List<OrderItem>>(orderItem, (JsonSerializerOptions)null),
                                  new ValueComparer<List<OrderItem>>(
                                      (c1, c2) => c1.SequenceEqual(c2),
                                             c => c.Aggregate(0, (a, orderItem) => HashCode.Combine(a, orderItem.GetHashCode())),
                                             c => c.ToList()
                                  ));
                   
            builder.Entity<OrderItem>().ToTable("OrderItems");
            builder.Entity<OrderItem>().HasKey((OrderItem   item) => new { item.OrderProduct, item.ProductId });
            builder.Entity<OrderItem>().Property((OrderItem item) => item.OrderId).IsRequired();
            builder.Entity<OrderItem>().Property((OrderItem item) => item.ProductId).IsRequired();             
            builder.Entity<OrderItem>().Property((OrderItem item) => item.OrderAmount).IsRequired();
            builder.Entity<OrderItem>().Property((OrderItem item) => item.SizePrice).HasColumnType("DECIMAL(5,2)");
            builder.Entity<OrderItem>().Property((OrderItem item) => item.Size).IsRequired();
            builder.Entity<OrderItem>().Property((OrderItem item) => item.OrderProduct)
                  .HasColumnName("ProductoPedido")
                  .HasConversion(product => JsonSerializer.Serialize(product, (JsonSerializerOptions)null),
                                 product => JsonSerializer.Deserialize<Product>(product, (JsonSerializerOptions)null)
                                );

            builder.Entity<Category>().ToTable("Categories");
            builder.Entity<Category>().HasKey((Category   category) => category.CategoryId);
            builder.Entity<Category>().Property((Category category) => category.CategoryName).IsRequired();
            builder.Entity<Category>().Property((Category category) => category.CategoryParentId).IsRequired();
        }
    }
}
