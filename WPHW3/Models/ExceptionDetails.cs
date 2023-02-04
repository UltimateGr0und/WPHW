using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WPHW3.Models
{
    public class ExceptionDetails
    {
        public string ExceptionMessage { get; set; }
        public string StackTrace { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public DateTime Date { get; set; }
    }
    public class RegistrationDetails
    {
        public string UserIp { get; set; }
        public DateTime Date { get; set; }
    }
}