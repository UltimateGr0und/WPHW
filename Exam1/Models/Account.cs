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
        [Required(ErrorMessage = "Username is required")]
        [MaxLength(20,ErrorMessage = "Username is too long")]
        [RegularExpression("^[a-zA-Z0-9]+$", ErrorMessage = "Unacceptable username")]
        [Display(Name = "Username")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [MinLength(4,ErrorMessage = "Password shoul contain at least 4 characters or numbers without special symbols")]
        [RegularExpression("^[a-zA-Z0-9]+$", ErrorMessage = "Password should contain at least 4 characters or numbers without special symbols")]
        public string Password { get; set; }
        public virtual ICollection<Session> Sessions { get; set; } = new List<Session>();
        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public virtual ICollection<ProductInfo> ProductsToSell { get; set; } = new List<ProductInfo>();
        public virtual ICollection<Product> ProductsToBuy { get; set; } = new List<Product>();
        public virtual ICollection<ProductInfo> AuctionLots { get; set; } = new List<ProductInfo>();
        public AccountType AccountType { get; set; }
        public Account() { }
    }
    public enum AccountType
    {
        Admin,
        Customer
    }
}