using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace WebApplication1.Models
{
    public class ProductDatabase : DbContext
    {
        public DbSet<Product> Products { get; set; }

    }

    public class Product
    {

        [Key]
        public int id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public int Price { get; set; }
        public string Image { get; set; }
    }


}