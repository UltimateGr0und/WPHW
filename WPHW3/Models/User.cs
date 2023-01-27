using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace WPHW3.Models
{
    public class User
    {
        [Key]
        [ForeignKey("Account")]
        public int Id { get; set; }
        public string FullName { get; set; }
        public int AccountId { get; set; }
        public virtual Account Account { get; set; }

    }
}