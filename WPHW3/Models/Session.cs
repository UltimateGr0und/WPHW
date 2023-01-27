using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WPHW3.Models
{
    public class Session
    {
        public int Id { get; set; }
        public virtual Account Account { get; set; }
        public string Ip { get; set; } //= HttpContext.Request.UserHostAddress;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}