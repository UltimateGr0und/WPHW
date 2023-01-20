using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;


namespace WPHW2.Models
{
    public class PersonDBContext : DbContext
    {
        public PersonDBContext()
            : base("DefaultConnection")
        {
        }

        public DbSet<Person> Employees { get; set; }
    }
}