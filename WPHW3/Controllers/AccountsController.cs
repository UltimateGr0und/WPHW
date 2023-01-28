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
        private Account RegistratedAccount()
        {
            try
            {
                return db.Accounts.Where(
                a => a.Sessions.Where(
                    s => s.Ip == HttpContext.Request.UserHostAddress).Any()).Single();
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
            Account account = RegistratedAccount();
            if (account == null) { return RedirectToAction("Index"); }
            if (account.AccountType!=AccountType.Patient) { return RedirectToAction("Index"); }
            
            return View(account.User);
        }
        public async Task<ActionResult> PatientAddDoctor()
        {
            Account account = RegistratedAccount();
            if (account == null) { return RedirectToAction("Index"); }
            if (account.AccountType!=AccountType.Patient) { return RedirectToAction("Index"); }

            Patient patient = (Patient)account.User;
            List<Doctor> awaibleDoctors = new List<Doctor>();
            List<int> UnAwaibleDoctorIds = patient.Doctors.Select(p => p.Id).ToList();
            awaibleDoctors.AddRange(db.Doctors.Where(d => !UnAwaibleDoctorIds.Contains(d.Id)));

            return View(awaibleDoctors);
        }
        public async Task<ActionResult> PatientDoAddDoctor(int Id)
        {
            Account account = RegistratedAccount();
            if (account == null) { return RedirectToAction("Index"); }
            if (account.AccountType != AccountType.Patient) { return RedirectToAction("Index"); }

            Patient patient = (Patient)account.User;
            Doctor doctor = (Doctor)db.Users.Where(d => d.Id == Id).First();
            patient.Doctors.Add(doctor);
            doctor.Patients.Add(patient);

            await db.SaveChangesAsync();

            return RedirectToAction("PatientMaster");
        }
        public async Task<ActionResult> PatientDoDeleteDoctor(int Id)
        {
            Account account = RegistratedAccount();
            if (account == null) { return RedirectToAction("Index"); }
            if (account.AccountType != AccountType.Patient) { return RedirectToAction("Index"); }

            Patient patient = (Patient)account.User;
            Doctor doctor = (Doctor)db.Users.Where(d => d.Id == Id).First();
            patient.Doctors.Remove(doctor);
            doctor.Patients.Remove(patient);

            await db.SaveChangesAsync();

            return RedirectToAction("PatientMaster");
        }
        public async Task<ActionResult> DoctorMaster()
        {
            Account account = RegistratedAccount();
            if (account == null) { return RedirectToAction("Index"); }
            if (account.AccountType != AccountType.Doctor) { return RedirectToAction("Index"); }

            return View((Doctor)account.User);
        }
        public async Task<ActionResult> AdminMaster(int DoctorsPageNumber = 1, int UsersPageNumber = 1)
        {
            Account account = RegistratedAccount();
            if (account == null) { return RedirectToAction("Index"); }
            if (account.AccountType != AccountType.Admin) { return RedirectToAction("Index"); }

            int PageSize = 5;

            AdminViewModel viewModel = new AdminViewModel
            {
                CurrentUser = account.User,
                DoctorsPageInfo = new PageInfo { PageNumber = DoctorsPageNumber, PageSize = PageSize, TotalItems = db.Doctors.Count() },
                Doctors = db.Doctors.OrderBy(d=>d.Id).Skip(PageSize * (DoctorsPageNumber - 1)).Take(PageSize),
                UsersPageInfo = new PageInfo { PageNumber = UsersPageNumber, PageSize = PageSize, TotalItems = db.Users.Count() },
                Users = db.Users.OrderBy(d=>d.Id).Skip(PageSize * (UsersPageNumber - 1)).Take(PageSize)
            };

            return View(viewModel);
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