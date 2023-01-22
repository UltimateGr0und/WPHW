using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using WPHW3.Models;

namespace WPHW3.Controllers
{
    public class AccountsController : Controller
    {
        private AccountDBContext db = new AccountDBContext();
        // GET: Accounts
        public ActionResult Index()
        {
            //List<Account> accounts = db.Accounts.Where(a => a.IsLocked == true).ToList();
            
            List<Account> accounts = db.Accounts.Where(
                a => a.Sessions.Where(
                    s => s.Ip == HttpContext.Request.UserHostAddress).Any()).ToList();
            if (accounts.Count == 1 && accounts.First().Sessions.Where(s => s.Ip == HttpContext.Request.UserHostAddress).First().EndTime<DateTime.Now)
            {
                ViewBag.Name = accounts.First().Name;
                return View();
            }
            else if (accounts.Count == 0)
            {
                return RedirectToAction("Registration");
            }
            else
            {
                foreach (var account in accounts)
                {
                    foreach (var session in account.Sessions)
                    {
                        db.Sessions.Remove(session);
                    }
                }
                db.SaveChanges();
                return RedirectToAction("Registration");
            }
        }

        public ActionResult Registration()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Registration(string username, string password, string submitButton)
        {
            Account account = new Account() { Name = username, Password = password };
            switch (submitButton)
            {
                case "SignUp":
                    if (db.Accounts.Where(a => a.Name == username).Count() == 0)
                    {
                        account.Sessions.Add(new Session() { Account = account, StartTime = DateTime.Now, EndTime = DateTime.Now.AddHours(1), Ip = HttpContext.Request.UserHostAddress });
                        db.Accounts.Add(account);
                        db.SaveChanges();
                    }
                    break;
                case "SignIn":
                    List<Account> currentAccounts = db.Accounts.Where(a => (a.Name == username && a.Password == password)).ToList();
                    if (currentAccounts.Count == 1)
                    {
                        currentAccounts.First().Sessions.Add(new Session() { Account = account, StartTime = DateTime.Now, EndTime = DateTime.Now.AddHours(1), Ip = HttpContext.Request.UserHostAddress });
                        db.SaveChanges();
                    }
                    break;
            }
            return RedirectToAction("Index");
        }
    }
}