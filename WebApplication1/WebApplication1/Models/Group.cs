using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace WebApplication1.Models
{
    public class GroupDatabase : DbContext
    {
        public DbSet<Group> Groups { get; set; }

    }

    public class Group
    {

        [Key]
        public int id { get; set; }
        public string Name { get; set; }
    }

}