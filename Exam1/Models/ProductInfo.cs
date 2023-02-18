using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        [Required(ErrorMessage = "Product name is required")]
        [MaxLength(30,ErrorMessage = "Too long product name")]
        [RegularExpression("^(?!\\s)(?!.*\\s$)(?=.*[a-zA-Z0-9])[a-zA-Z0-9 '~?!]{2,}$",ErrorMessage = "Unacceptable product name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Descrotption is required")]
        [MaxLength(400,ErrorMessage = "Too long description")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Price is required")]
        [Range(0,double.MaxValue,ErrorMessage = "Invalid price")]
        public double Price { get; set; }
        [Required(ErrorMessage = "Amount of available products is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Invalid amount of available products")]
        [DisplayName("Available")]
        public int TotalAmount { get; set; }
        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
        public virtual Account Seller { get; set; }
    }
}