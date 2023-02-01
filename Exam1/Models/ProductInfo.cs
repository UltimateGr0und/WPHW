using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Exam1.Models
{
    public class ProductInfo
    {
        [HiddenInput(DisplayValue =false)]
        public int Id { get; set; }
        [Required]
        [MaxLength(30)]
        public string Name { get; set; }
        [Required]
        [MaxLength(400)]
        public string Description { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        public int TotalAmount { get; set; }
        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
        public virtual Account Seller { get; set; }
    }
}