using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace WebApplication1.Models
{
    public class CartDatabase : DbContext
    {
        public DbSet<Cart> Carts { get; set; }

    }

    public class Cart
    {

        [Key]
        public int id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
    }

}