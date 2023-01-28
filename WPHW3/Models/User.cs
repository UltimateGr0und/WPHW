using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WPHW3.Models
{
    public class User
    {
        [Key]
        [ForeignKey("Account")]
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }
        public string FullName { get; set; }
        public int AccountId { get; set; }
        public virtual Account Account { get; set; }

    }
}