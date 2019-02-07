using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace WebApplication1.Models
{
    public class OrderDatabase : DbContext
    {
        public DbSet<Order> Orders { get; set; }

    }

    public class Order
    {

        [Key]
        public int id { get; set; }
        public string Goods { get; set; }
        public int TotalPrice { get; set; }
        public string OrderDate { get; set; }
        public int Status { get;set; }
        public int userid { get; set; }
    }

}