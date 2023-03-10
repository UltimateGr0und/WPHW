using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc.Filters;
using System.Web.Mvc;
using Exam1.Models;

namespace Exam1.Filters
{
    public class AdminAuthentificationFilter : FilterAttribute, IAuthenticationFilter
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
            if (account == null || account.AccountType != AccountType.Admin)
            {
                filterContext.Result = new HttpUnauthorizedResult();
            }
        }
        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
            Account account = RegistratedAccount(filterContext.HttpContext.Request.UserHostAddress);
            if (account == null || account.AccountType != AccountType.Admin)
            {
                filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary { { "controller", "Registration" }, { "action", "Index" } });
            }
        }
    }
}