using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WPHW3.Models
{
    public class Account
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public virtual ICollection<Session> Sessions { get; set; }
        public AccountType AccountType { get; set; }
        public virtual User User { get; set; }
        public Account() { Sessions = new List<Session>(); }
    }
    public enum AccountType
    {
        Admin,
        Doctor,
        Patient
    }
}