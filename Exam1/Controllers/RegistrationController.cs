using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Exam1.Models;
using Microsoft.AspNet.Identity;

namespace Exam1.Controllers
{
    public class RegistrationController : Controller
    {
        private AccountDBContext db = new AccountDBContext();
        // GET: Accounts
        private Account RegistratedAccount()
        {
            try
            {
                return db.Accounts.Where(
                a => a.Sessions.Where(
                    s => s.Ip == HttpContext.Request.UserHostAddress).Where(s => s.EndTime > DateTime.UtcNow).Any()).Single();
            }
            catch (Exception)
            {
                return null;
            }
        }
        private List<Account> RegistratedAccounts()
        {
            return db.Accounts.Where(
                a => a.Sessions.Where(
                    s => s.Ip == HttpContext.Request.UserHostAddress).Where(s => s.EndTime > DateTime.UtcNow).Any()).ToList();
        }
        private void FixRegistration()
        {
            List<Account> accounts = RegistratedAccounts();
            foreach (var account in accounts)
            {
                //var res = db.Sessions.Where(s => s.Ip == HttpContext.Request.UserHostAddress).Where(s => s.EndTime > DateTime.UtcNow);
                //db.Sessions.RemoveRange(res);
                foreach (var s in account.Sessions)
                {
                    s.EndTime = DateTime.UtcNow;
                }
            }
            db.SaveChanges();
        }
        public async Task<ActionResult> Index()
        {
            //List<Account> accounts = db.Accounts.Where(a => a.IsLocked == true).ToList();

            List<Account> accounts = RegistratedAccounts();
            if (accounts.Count == 1 /*&& accounts.First().Sessions.Where(s => s.Ip == HttpContext.Request.UserHostAddress).First().EndTime<DateTime.UtcNow*/)
            {
                Account account = accounts.First();
                AccountType type = account.AccountType;
                switch (type)
                {
                    case AccountType.Admin:
                        return RedirectToAction("AccountIndex","Admin");
                    case AccountType.Customer:
                        return RedirectToAction("Index","Customer");
                    default:
                        return RedirectToAction("SignIn");
                }
            }
            else if (accounts.Count == 0)
            {
                return RedirectToAction("SignIn");
            }
            else
            {
                FixRegistration();
                return RedirectToAction("SignIn");
            }
        }
        public async Task<ActionResult> SignIn()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> SignIn(Account account)
        {
            if (ModelState.IsValid)
            {
                List<Account> currentAccounts = db.Accounts.Where(a => (a.Username == account.Username)).ToList();
                if (currentAccounts.Count == 1)
                {
                    PasswordHasher ph = new PasswordHasher();
                    switch (ph.VerifyHashedPassword(currentAccounts.First().Password, account.Password))
                    {
                        case PasswordVerificationResult.Success:
                            {
                                Session session = new Session() { Account = currentAccounts.First(), StartTime = DateTime.UtcNow, EndTime = DateTime.UtcNow.AddHours(1), Ip = HttpContext.Request.UserHostAddress };
                                db.Sessions.Add(session);
                                currentAccounts.First().Sessions.Add(session);
                                db.SaveChanges();
                                return RedirectToAction("Index");
                            }
                        case PasswordVerificationResult.Failed:
                            {
                                TempData["message"] = "invalid password or username";
                                return RedirectToAction("SignIn");
                            }
                        case PasswordVerificationResult.SuccessRehashNeeded:
                            {
                                currentAccounts.First().Password = ph.HashPassword(account.Password);
                                Session session = new Session() { Account = currentAccounts.First(), StartTime = DateTime.UtcNow, EndTime = DateTime.UtcNow.AddHours(1), Ip = HttpContext.Request.UserHostAddress };
                                db.Sessions.Add(session);
                                currentAccounts.First().Sessions.Add(session);
                                db.SaveChanges();
                                return RedirectToAction("Index");
                            }


                    }

                }
                else
                {
                    TempData["message"] = "invalid password or username";
                    return RedirectToAction("SignIn");  
                }
            }
            return RedirectToAction("SignIn");
        }
        public async Task<ActionResult> SignUp()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> SignUp(Account account)
        {
            if (db.Accounts.Where(a => a.Username == account.Username).Count() == 0)
            {
                account.AccountType = AccountType.Customer;
                PasswordHasher ph = new PasswordHasher();
                account.Password = ph.HashPassword(account.Password);
                
                Session session = new Session() { Ip = HttpContext.Request.UserHostAddress, Account = account, StartTime = DateTime.UtcNow, EndTime = DateTime.UtcNow.AddHours(1) };
                db.Sessions.Add(session);
                db.Accounts.Add(account);
                account.Sessions.Add(session);
                
                db.SaveChanges();
            }
            else
            {
                TempData["message"] = "occupied username";
                return RedirectToAction("SignUp");
            }
            return RedirectToAction("Index");
        }
        public async Task<ActionResult> LogOut()
        {
            var res = db.Sessions.Where(s => s.Ip == HttpContext.Request.UserHostAddress).Where(s=>s.EndTime>DateTime.UtcNow);
            //db.Sessions.RemoveRange(res);
            foreach (var s in res)
            {
                s.EndTime = DateTime.UtcNow;
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}