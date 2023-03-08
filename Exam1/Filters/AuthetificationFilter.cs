using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;
using Exam1.Controllers;
using Exam1.Models;

namespace Exam1.Filters
{
    internal class AuthetificationFilter:FilterAttribute,IAuthenticationFilter
    {
        private AccountDBContext db = new AccountDBContext();
        // GET: Accounts
        private Account RegistratedAccount(string ip)
        {
            try
            {
                return db.Accounts.Where(
                a => a.Sessions.Where(
                    s => s.Ip == ip).Any()).Single();
            }
            catch (Exception)
            {
                return null;
            }
        }
        public void OnAuthentication(AuthenticationContext filterContext)
        {
            Account account = RegistratedAccount(filterContext.HttpContext.Request.UserHostAddress);
            if (account == null || account.AccountType != AccountType.Customer)
            {
                filterContext.Result = new HttpUnauthorizedResult();
            }
        }
        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
            Account account = RegistratedAccount(filterContext.HttpContext.Request.UserHostAddress);
            if (account == null || account.AccountType != AccountType.Customer)
            {
                filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary { { "controller", "Registration" }, { "action", "Index" } });
            }
        }
    }
}