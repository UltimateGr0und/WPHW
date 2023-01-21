using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace WPHW3.Models
{
    public class AccountDBContext:DbContext
    {
        public AccountDBContext():base("DefaultConnection") { }
        public DbSet<Account> Accounts { get; set; }
    }
}