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
        public List<Session> Sessions { get; set; }
        public AccountType AccountType { get; set; }
    }
    public enum AccountType
    {
        Admin,
        Doctor,
        Patient
    }
}