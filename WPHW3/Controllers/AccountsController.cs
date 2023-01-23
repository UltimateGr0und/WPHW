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
        private List<Account> RegistratedAccounts()
        {
            return db.Accounts.Where(
                a => a.Sessions.Where(
                    s => s.Ip == HttpContext.Request.UserHostAddress).Any()).ToList();
        }
        private void FixRegistration()
        {
            List<Account> accounts = RegistratedAccounts();
            foreach (var account in accounts)
            {
                foreach (var session in account.Sessions)
                {
                    db.Sessions.Remove(session);
                }
            }
            db.SaveChanges();
        }
        private bool CheckUser(AccountType type)
        {
            if (RegistratedAccounts().First().AccountType==type)
            {
                return true;
            }
            return false;
        }
        public async Task<ActionResult> Index()
        {
            //List<Account> accounts = db.Accounts.Where(a => a.IsLocked == true).ToList();

            List<Account> accounts = RegistratedAccounts();
            if (accounts.Count == 1 /*&& accounts.First().Sessions.Where(s => s.Ip == HttpContext.Request.UserHostAddress).First().EndTime<DateTime.Now*/)
            {
                Account account = accounts.First();
                AccountType type = account.AccountType;
                switch (type)
                {
                    case AccountType.Admin:
                        return RedirectToAction("AdminMaster");
                    case AccountType.Doctor:
                        return RedirectToAction("DoctorMaster");
                    case AccountType.Patient:
                        return RedirectToAction("PatientMaster");
                    default:
                        return RedirectToAction("Registration");
                }
            }
            else if (accounts.Count == 0)
            {
                return RedirectToAction("Registration");
            }
            else
            {
                FixRegistration();
                return RedirectToAction("Registration");
            }
        }
        public async Task<ActionResult> PatientMaster()
        {
            if (!CheckUser(AccountType.Patient)) { RedirectToAction("Index"); }
            Patient patient = (Patient)RegistratedAccounts().First().User;

            return View(patient);
        }
        public async Task<ActionResult> DoctorMaster()
        {
            if (!CheckUser(AccountType.Doctor)) { RedirectToAction("Index"); }
            Doctor doctor = (Doctor)RegistratedAccounts().First().User;

            return View(doctor);
        }
        public async Task<ActionResult> AdminMaster()
        {
            if (!CheckUser(AccountType.Admin)) { RedirectToAction("Index"); }
            User admin = db.Users.Where(u => u.Account.Sessions.Where(s => s.Ip == HttpContext.Request.UserHostAddress).Any()).First();

            return View(admin);
        }
        [HttpPost] 
        public async Task<ActionResult> CreateDoctor(string username,string password,string fullname,string description)
        {
            Doctor doctor = new Doctor() { FullName = fullname, Description = description };
            Account account = new Account() { AccountType= AccountType.Doctor, Name=username, Password=password };
            Session session = new Session() { Ip = HttpContext.Request.UserHostAddress, StartTime = DateTime.Now, EndTime = DateTime.Now.AddHours(1) };
            db.Sessions.Add(session);
            db.Accounts.Add(account);
            db.Doctors.Add(doctor);
            account.Sessions.Add(session);
            session.Account = account;
            account.User = doctor;
            doctor.Account = account;
            db.SaveChanges();

            return RedirectToAction("AdminMaster");
        }
        public async Task<ActionResult> Registration()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Registration(string username, string password, string submitButton)
        {
            if (RegistratedAccounts().Count != 0)
            {
                FixRegistration();
            }
            Account account = new Account() { Name = username, Password = password };
            switch (submitButton)
            {
                case "SignUp":
                    if (db.Accounts.Where(a => a.Name == username).Count() == 0)
                    {
                        Session session = new Session() { Ip = HttpContext.Request.UserHostAddress, Account = account, StartTime = DateTime.Now, EndTime = DateTime.Now.AddHours(1) };
                        db.Sessions.Add(session);
                        account.AccountType = AccountType.Patient;
                        db.Accounts.Add(account);
                        account.Sessions.Add(session);
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