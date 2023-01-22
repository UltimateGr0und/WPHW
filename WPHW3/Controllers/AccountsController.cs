using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public async Task<ActionResult> Index()
        {
            //List<Account> accounts = db.Accounts.Where(a => a.IsLocked == true).ToList();
            
            List<Account> accounts = db.Accounts.Where(
                a => a.Sessions.Where(
                    s => s.Ip == HttpContext.Request.UserHostAddress).Any()).ToList();
            if (accounts.Count == 1 /*&& accounts.First().Sessions.Where(s => s.Ip == HttpContext.Request.UserHostAddress).First().EndTime<DateTime.Now*/)
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

        public async Task<ActionResult> Registration()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Registration(string username, string password, string submitButton)
        {
            Account account = new Account() { Name = username, Password = password };
            switch (submitButton)
            {
                case "SignUp":
                    if (db.Accounts.Where(a => a.Name == username).Count() == 0)
                    {
                        Session session = new Session() { Ip = HttpContext.Request.UserHostAddress, Account = account, StartTime = DateTime.Now, EndTime = DateTime.Now.AddHours(1) };
                        db.Sessions.Add(session);
                        account.Sessions.Add(session);
                        db.Accounts.Add(account);
                        db.SaveChanges();
                    }
                    break;
                case "SignIn":
                    List<Account> currentAccounts = db.Accounts.Where(a => (a.Name == username && a.Password == password)).ToList();
                    if (currentAccounts.Count == 1)
                    {
                        Session session = new Session() { Account = currentAccounts.First(), StartTime = DateTime.Now, EndTime = DateTime.Now.AddHours(1), Ip = HttpContext.Request.UserHostAddress };
                        db.Sessions.Add(session);
                        currentAccounts.First().Sessions.Add(session);
                        db.SaveChanges();
                    }
                    break;
            }
            return RedirectToAction("Index");
        }
        public async Task<ActionResult> LogOut()
        {            
            Account account = db.Accounts.Where(
                a => a.Sessions.Where(
                    s => s.Ip == HttpContext.Request.UserHostAddress).Any()).First();
            foreach (var item in db.Sessions) { }
            Session session = account.Sessions.Where(s => s.Ip == HttpContext.Request.UserHostAddress).First();
            account.Sessions.Remove(session);
            db.SaveChanges();
            return RedirectToAction("Registration");
        }
    }
}