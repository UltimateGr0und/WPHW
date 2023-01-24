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
            if (RegistratedAccounts().Count != 1) { return RedirectToAction("Index"); }
            if (!CheckUser(AccountType.Patient)) { return RedirectToAction("Index"); }
            
            Patient patient = db.Patients.Where(u => u.Account.Sessions.Where(s => s.Ip == HttpContext.Request.UserHostAddress).Any()).First();
            foreach (var d in db.Doctors)
            {
                string temp = d.FullName;
            }
            return View(patient);
        }
        public async Task<ActionResult> PatientAddDoctor()
        {
            if (RegistratedAccounts().Count != 1) { return RedirectToAction("Index"); }
            if (!CheckUser(AccountType.Patient)) { return RedirectToAction("Index"); }
            Patient patient = db.Patients.Where(u => u.Account.Sessions.Where(s => s.Ip == HttpContext.Request.UserHostAddress).Any()).First();

            List<Doctor> awaibleDoctors = new List<Doctor>();
            awaibleDoctors.AddRange(db.Doctors.Where(d=>!patient.Doctors.Contains(d)));

            return View(awaibleDoctors);
        }
        public async Task<ActionResult> PatientDoAddDoctor(int Id)
        {
            if (RegistratedAccounts().Count != 1) { return RedirectToAction("Index"); }
            if (!CheckUser(AccountType.Patient)) { return RedirectToAction("Index"); }
            Patient patient = db.Patients.Where(u => u.Account.Sessions.Where(s => s.Ip == HttpContext.Request.UserHostAddress).Any()).First();

            Doctor doctor = (Doctor)db.Users.Where(d => d.Id == Id).First();
            patient.Doctors.Add(doctor);
            doctor.Patients.Add(patient);

            //db.Entry(doctor).State = System.Data.Entity.EntityState.Modified;
            //db.Entry(patient).State = System.Data.Entity.EntityState.Modified;

            await db.SaveChangesAsync();

            return RedirectToAction("PatientMaster");
        }
        public async Task<ActionResult> DoctorMaster()
        {
            if (RegistratedAccounts().Count != 1) { return RedirectToAction("Index"); }
            if (!CheckUser(AccountType.Doctor)) { return RedirectToAction("Index"); }
            Doctor doctor = db.Doctors.Where(u => u.Account.Sessions.Where(s => s.Ip == HttpContext.Request.UserHostAddress).Any()).First();

            return View(doctor);
        }
        public async Task<ActionResult> AdminMaster()
        {
            if (RegistratedAccounts().Count != 1) { return RedirectToAction("Index"); }
            if (!CheckUser(AccountType.Admin)) { return RedirectToAction("Index"); }
            User admin = db.Users.Where(u => u.Account.Sessions.Where(s => s.Ip == HttpContext.Request.UserHostAddress).Any()).First();

            return View(admin);
        }
        [HttpPost] 
        public async Task<ActionResult> CreateDoctor(string username,string password,string fullname,string description)
        {
            if (db.Accounts.Where(a => a.Name == username).Count()!=0)
            {
                return RedirectToAction("AdminMaster");
            }
            Doctor doctor = new Doctor() { FullName = fullname, Description = description };
            Account account = new Account() { AccountType= AccountType.Doctor, Name=username, Password=password };
            db.Accounts.Add(account);
            db.Doctors.Add(doctor);
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
                        Patient patient = new Patient() { FullName = username };
                        account.AccountType = AccountType.Patient;

                        db.Patients.Add(patient);
                        db.Sessions.Add(session);
                        db.Accounts.Add(account);
                        account.Sessions.Add(session);
                        account.User = patient;
                        patient.Account = account;
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