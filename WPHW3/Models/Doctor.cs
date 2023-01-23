using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WPHW3.Models
{
    public class Doctor:User
    {
        public string Description { get; set; }
        public ICollection<Patient> Patients { get; set; } = new List<Patient>();
    }
}