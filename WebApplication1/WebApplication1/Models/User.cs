using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace WebApplication1.Models
{
    public class UserDatabase : DbContext
    {
        public DbSet<User> Users { get; set; }

    }

    public class User
    {

        [Key]
        public int id { get; set; }
        public string Username { get; set; }
        public int IsAdmin { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Orders { get; set; }
        public int IsLogged { get; set; }
        public string Cart { get; set; }
    }

}