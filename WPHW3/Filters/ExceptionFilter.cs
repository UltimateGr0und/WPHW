using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WPHW3.Models;

namespace WPHW3.Filters
{
    public class ExceptionFilter : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            ExceptionDetails exceptionDetail = new ExceptionDetails()
            {
                ExceptionMessage = filterContext.Exception.Message,
                StackTrace = filterContext.Exception.StackTrace,
                ControllerName = filterContext.RouteData.Values["Controller"].ToString(),
                ActionName = filterContext.RouteData.Values["Action"].ToString(),
                Date = DateTime.UtcNow
            };

            using (LogDBContext db = new LogDBContext())
            {
                db.ExceptionDetails.Add(exceptionDetail);
                db.SaveChanges();
            }

            filterContext.ExceptionHandled = true;

            /*
                            if (!filterContext.ExceptionHandled && filterContext.Exception is IndexOutOfRangeException)
                            {
                                filterContext.Result = new RedirectResult("~/Shared/Error");
                            }*/
        }
    }
}