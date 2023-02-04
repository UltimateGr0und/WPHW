using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace WPHW3.Models
{
    public class LogDBContext:DbContext
    {
        public LogDBContext() : base() { }
        public DbSet<ExceptionDetails> ExceptionDetails { get; set; }
        public DbSet<RegistrationDetails> RegistrationDetails { get; set; }
    }
}