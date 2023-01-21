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
            List<Account> accounts = db.Accounts.Where(a => a.IsLocked == true).ToList();
            if (accounts.Count == 1)
            {
                ViewBag.Name = accounts.First().Name;
            }
            else if (accounts.Count == 0)
            {
                return RedirectToAction("Registration");
            }
            else
            {
                foreach (var account in db.Accounts) { account.IsLocked = false; }
                return RedirectToAction("Registration");
            }
            return View();
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
                        account.IsLocked = true;
                        db.Accounts.Add(account);
                        db.SaveChanges();
                    }
                    break;
                case "SignIn":
                    List<Account> currentAccounts = db.Accounts.Where(a => (a.Name == username && a.Password == password)).ToList();
                    if (currentAccounts.Count == 1)
                    {
                        currentAccounts.First().IsLocked = true;
                        db.SaveChanges();
                    }
                    break;
            }
            return RedirectToAction("Index");
        }
    }
}