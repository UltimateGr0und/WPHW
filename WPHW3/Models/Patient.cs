using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WPHW3.Models
{
    public class Patient:User
    {
        public virtual ICollection<Doctor> Doctors { get; set; }
        public Patient()
        {
            Doctors = new List<Doctor>();
        }
    }
}