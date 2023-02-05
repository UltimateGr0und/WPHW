using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Exam1.Models
{
    public class Account
    {        
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }
        [Required]
        [MaxLength(20)]
        [Display(Name = "Name")]
        public string Username { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [MinLength(4)]
        [MaxLength(20)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        public virtual ICollection<Session> Sessions { get; set; } = new List<Session>();
        public virtual ICollection<ProductInfo> ProductsToSell { get; set; } = new List<ProductInfo>();
        public virtual ICollection<Product> ProductsToBuy { get; set; } = new List<Product>();
        public AccountType AccountType { get; set; }
        public Account() { }
    }
    public enum AccountType
    {
        Admin,
        Customer
    }
}