using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Exam1.Models
{
    public class AccountDBContext:DbContext
    {
        public AccountDBContext() : base() { }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductInfo> ProductInfos { get; set; }

    }
}