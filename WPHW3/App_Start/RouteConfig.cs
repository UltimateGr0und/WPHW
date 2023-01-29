using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WPHW3
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("Accounts/AdminMaster");

            routes.MapRoute(
                name: "EtoNeVhodDlyaAdmina",
                url: "Accounts/EtoNeVhodDlyaAdmina/{DoctorsPageNumber}/{UsersPageNumber}/{AnyPatients}/{AnySessions}",
                new { controller = "Accounts", action = "AdminMaster", DoctorsPageNumber = UrlParameter.Optional, UsersPageNumber = UrlParameter.Optional, AnyPatients = UrlParameter.Optional, AnySessions = UrlParameter.Optional, }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Accounts", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
