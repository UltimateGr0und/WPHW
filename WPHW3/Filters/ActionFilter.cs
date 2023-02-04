using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WPHW3.Models;

namespace WPHW3.Filters
{
    public class ActionFilter : FilterAttribute, IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.HttpContext.Response.Write("Current User:" + filterContext.HttpContext.User.Identity.Name);

        }
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            filterContext.HttpContext.Response.Write("Time of current request:" + filterContext.HttpContext.Timestamp);
            LogDBContext db = new LogDBContext();

            RegistrationDetails details = new RegistrationDetails { Date = DateTime.UtcNow, UserIp = filterContext.HttpContext.Request.UserHostAddress };

            db.RegistrationDetails.Add(details);
            db.SaveChanges();
        }
    }
}