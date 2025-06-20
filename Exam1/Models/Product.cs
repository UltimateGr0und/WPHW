using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Exam1.Models
{
    public class Product
    {
        public int Id { get; set; }
        public virtual ProductInfo ProductInfo { get; set; }
        public int Amount { get; set; }
        public virtual Account Owner { get; set; }
    }
}