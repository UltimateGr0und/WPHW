using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WPHW2.Controllers
{
    public class InfoController : Controller
    {
        // GET: Info
        public string Index()
        {
            HttpContext.Response.Write("<h1>Welcome to HttpContext</h1>");

            string browser = HttpContext.Request.Browser.Browser;
            string user_agent = HttpContext.Request.UserAgent;
            string url = HttpContext.Request.RawUrl;
            string ip = HttpContext.Request.UserHostAddress;
            string referrer = HttpContext.Request.UrlReferrer == null ? "" : HttpContext.Request.UrlReferrer.AbsoluteUri;

            return "<p> Browser: " + browser + "</p> <p>User-Agent: " + user_agent + "</p><p>Url запрос: " + url + "</p> <p> Реферрер: " + referrer + "</p><p>IP: " + ip + "</p>";
        }
    }
}