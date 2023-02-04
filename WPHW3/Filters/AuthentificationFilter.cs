using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;
using WPHW3.Models;

namespace WPHW3.Filters
{
    public class AdminAuthentificationFilter:FilterAttribute, IAuthenticationFilter
    {
        protected AccountDBContext db = new AccountDBContext();
        // GET: Accounts
        protected Account RegistratedAccount(string Ip)
        {
            try
            {
                return db.Accounts.Where(
                a => a.Sessions.Where(
                    s => s.Ip == Ip).Any()).Single();
            }
            catch (Exception)
            {
                return null;
            }
        }
        protected List<Account> RegistratedAccounts(string Ip)
        {
            return db.Accounts.Where(
                a => a.Sessions.Where(
                    s => s.Ip == Ip).Any()).ToList();
        }
        protected void FixRegistration(string Ip)
        {
            List<Account> accounts = RegistratedAccounts(Ip);
            foreach (var account in accounts)
            {
                foreach (var session in account.Sessions)
                {
                    db.Sessions.Remove(session);
                }
            }
            db.SaveChanges();
        }
        public void OnAuthentication(AuthenticationContext filterContext)
        {
            Account account = RegistratedAccount(filterContext.HttpContext.Request.UserHostAddress);
            if (account == null||account.AccountType!=AccountType.Admin)
            {
                filterContext.Result = new HttpUnauthorizedResult();
            }
        }
        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
            Account account = RegistratedAccount(filterContext.HttpContext.Request.UserHostAddress);
            if (account == null || account.AccountType != AccountType.Admin)
            {
                filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary { { "controller", "Accounts" }, { "action", "Index" } });
            }
        }
    }
    public class DoctorAuthentificationFilter:FilterAttribute, IAuthenticationFilter
    {
        protected AccountDBContext db = new AccountDBContext();
        // GET: Accounts
        protected Account RegistratedAccount(string Ip)
        {
            try
            {
                return db.Accounts.Where(
                a => a.Sessions.Where(
                    s => s.Ip == Ip).Any()).Single();
            }
            catch (Exception)
            {
                return null;
            }
        }
        protected List<Account> RegistratedAccounts(string Ip)
        {
            return db.Accounts.Where(
                a => a.Sessions.Where(
                    s => s.Ip == Ip).Any()).ToList();
        }
        protected void FixRegistration(string Ip)
        {
            List<Account> accounts = RegistratedAccounts(Ip);
            foreach (var account in accounts)
            {
                foreach (var session in account.Sessions)
                {
                    db.Sessions.Remove(session);
                }
            }
            db.SaveChanges();
        }
        public void OnAuthentication(AuthenticationContext filterContext)
        {
            Account account = RegistratedAccount(filterContext.HttpContext.Request.UserHostAddress);
            if (account == null||account.AccountType!=AccountType.Doctor)
            {
                filterContext.Result = new HttpUnauthorizedResult();
            }
        }
        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
            Account account = RegistratedAccount(filterContext.HttpContext.Request.UserHostAddress);
            if (account == null || account.AccountType != AccountType.Doctor)
            {
                filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary { { "controller", "Accounts" }, { "action", "Index" } });
            }
        }
    }
    public class PatientAuthentificationFilter:FilterAttribute, IAuthenticationFilter
    {
        protected AccountDBContext db = new AccountDBContext();
        // GET: Accounts
        protected Account RegistratedAccount(string Ip)
        {
            try
            {
                return db.Accounts.Where(
                a => a.Sessions.Where(
                    s => s.Ip == Ip).Any()).Single();
            }
            catch (Exception)
            {
                return null;
            }
        }
        protected List<Account> RegistratedAccounts(string Ip)
        {
            return db.Accounts.Where(
                a => a.Sessions.Where(
                    s => s.Ip == Ip).Any()).ToList();
        }
        protected void FixRegistration(string Ip)
        {
            List<Account> accounts = RegistratedAccounts(Ip);
            foreach (var account in accounts)
            {
                foreach (var session in account.Sessions)
                {
                    db.Sessions.Remove(session);
                }
            }
            db.SaveChanges();
        }
        public void OnAuthentication(AuthenticationContext filterContext)
        {
            Account account = RegistratedAccount(filterContext.HttpContext.Request.UserHostAddress);
            if (account == null||account.AccountType!=AccountType.Patient)
            {
                filterContext.Result = new HttpUnauthorizedResult();
            }
        }
        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
            Account account = RegistratedAccount(filterContext.HttpContext.Request.UserHostAddress);
            if (account == null || account.AccountType != AccountType.Patient)
            {
                filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary { { "controller", "Accounts" }, { "action", "Index" } });
            }
        }
    }
    //public class DoctorAuthentificationFilter : AdminAuthentificationFilter
    //{
    //    public new void OnAuthentication(AuthenticationContext filterContext)
    //    {
    //        Account account = RegistratedAccount(filterContext.HttpContext.Request.UserHostAddress);
    //        if (account == null || account.AccountType != AccountType.Doctor)
    //        {
    //            filterContext.Result = new HttpUnauthorizedResult();
    //        }
    //    }
    //    public new void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
    //    {
    //        Account account = RegistratedAccount(filterContext.HttpContext.Request.UserHostAddress);
    //        if (account == null || account.AccountType != AccountType.Doctor)
    //        {
    //            filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary { { "controller", "Accounts" }, { "action", "Index" } });
    //        }
    //    }
    //}

}