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
        [MaxLength(100,ErrorMessage = "Too long product name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Description is required")]
        [MaxLength(4000,ErrorMessage = "Too long description")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Category is required")]
        public string Category { get; set; }
        [DataType(DataType.Upload)]
        [Display(Name = "Upload File")]
        [Required(ErrorMessage = "Please choose file to upload.")]
        public string Photo { get; set; } = "https://whey.kz/wp-content/uploads/2020/11/placeholder.png";
        [Required(ErrorMessage = "Price is required")]
        [Range(0,double.MaxValue,ErrorMessage = "Invalid price")]
        public double Price { get; set; }
        [Required(ErrorMessage = "Amount of available products is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Invalid amount of available products")]
        [DisplayName("Available")]
        public int TotalAmount { get; set; }
        public string IsAuction { get; set; } = "product";
        public virtual ICollection<Account> Applicants { get; set; }
        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
        public virtual ICollection<Comment> Comments { get; set; }  =new List<Comment>();
        public virtual Account Seller { get; set; }
    }
}