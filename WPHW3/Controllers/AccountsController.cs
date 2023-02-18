using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Xml.Linq;
using WPHW3.Filters;
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
        [ExceptionFilter]
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
                        return RedirectToAction("EtoNeVhodDlyaAdmina");
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
        [CacheAttribute]
        [PatientAuthentificationFilter]
        public async Task<ActionResult> PatientMaster()
        {
            Account account = RegistratedAccount();
            
            return View(account.User);
        }
        [PatientAuthentificationFilter]
        public async Task<ActionResult> PatientAddDoctor()
        {
            Account account = RegistratedAccount();

            Patient patient = (Patient)account.User;
            List<Doctor> awaibleDoctors = new List<Doctor>();
            List<int> UnAwaibleDoctorIds = patient.Doctors.Select(p => p.Id).ToList();
            awaibleDoctors.AddRange(db.Doctors.Where(d => !UnAwaibleDoctorIds.Contains(d.Id)));

            return View(awaibleDoctors);
        }
        [PatientAuthentificationFilter]
        public async Task<ActionResult> PatientDoAddDoctor(int Id)
        {
            Account account = RegistratedAccount();

            Patient patient = (Patient)account.User;
            Doctor doctor = (Doctor)db.Users.Where(d => d.Id == Id).First();
            patient.Doctors.Add(doctor);
            doctor.Patients.Add(patient);

            await db.SaveChangesAsync();

            return RedirectToAction("PatientMaster");
        }
        [PatientAuthentificationFilter]
        public async Task<ActionResult> PatientDoDeleteDoctor(int Id)
        {
            Account account = RegistratedAccount();

            Patient patient = (Patient)account.User;
            Doctor doctor = (Doctor)db.Users.Where(d => d.Id == Id).First();
            patient.Doctors.Remove(doctor);
            doctor.Patients.Remove(patient);

            await db.SaveChangesAsync();

            return RedirectToAction("PatientMaster");
        }
        [DoctorAuthentificationFilter]
        [CacheAttribute]
        public async Task<ActionResult> DoctorMaster()
        {
            Account account = RegistratedAccount();

            return View((Doctor)account.User);
        }
        [AdminAuthentificationFilter]
        public async Task<ActionResult> AdminMaster(int DoctorsPageNumber = 1, int UsersPageNumber = 1, string AnyPatients = "none", string AnySessions="none", string DoctorNameFilter="")
        {
            Account account = RegistratedAccount();
            
            DoctorsFilterInfo doctorsFilterInfo = new DoctorsFilterInfo() { DoctorNameFilter=DoctorNameFilter };
            switch (AnyPatients)
            {
                case "none":
                    doctorsFilterInfo.AnyPatients = "None";
                    break;
                case "with patients":
                    doctorsFilterInfo.AnyPatients = "With";
                    break;
                case "without patients":
                    doctorsFilterInfo.AnyPatients = "Without";
                    break;
                default:
                    break;
            }
            UsersFilterInfo usersFilterInfo = new UsersFilterInfo();
            switch (AnySessions)
            {
                case "none":
                    usersFilterInfo.AnySessions = "None";
                    break;
                case "with sessions":
                    usersFilterInfo.AnySessions = "With";
                    break;
                case "without sessions":
                    usersFilterInfo.AnySessions = "Without";
                    break;
                default:
                    break;
            }
            int PageSize = 5;
            if (usersFilterInfo == null) { usersFilterInfo = new UsersFilterInfo(); }
            if(doctorsFilterInfo == null) { doctorsFilterInfo = new DoctorsFilterInfo(); }

            AdminViewModel viewModel = new AdminViewModel
            {
                CurrentUser = account.User,
                DoctorsPageInfo = new PageInfo { PageNumber = DoctorsPageNumber, PageSize = PageSize, TotalItems = db.Doctors.Count() },
                UsersPageInfo = new PageInfo { PageNumber = UsersPageNumber, PageSize = PageSize, TotalItems = db.Users.Count() },
                doctorsFilterInfo = doctorsFilterInfo,
                usersFilterInfo= usersFilterInfo
            };
            switch (usersFilterInfo.AnySessions)
            {
                case "None":
                    viewModel.Users = db.Users.OrderBy(d => d.Id).Skip(PageSize * (UsersPageNumber - 1)).Take(PageSize);
                    viewModel.UsersPageInfo.TotalItems = db.Users.Count();
                    break;
                case "With":
                    viewModel.Users = db.Users.Where(u => u.Account.Sessions.Any()).OrderBy(d => d.Id).Skip(PageSize * (UsersPageNumber - 1)).Take(PageSize);
                    viewModel.UsersPageInfo.TotalItems = db.Users.Where(u => u.Account.Sessions.Any()).Count();
                    break;
                case "Without":
                    viewModel.Users = db.Users.Where(u => !u.Account.Sessions.Any()).OrderBy(d => d.Id).Skip(PageSize * (UsersPageNumber - 1)).Take(PageSize);
                    viewModel.UsersPageInfo.TotalItems = db.Users.Where(u => !u.Account.Sessions.Any()).Count();
                    break;
                default:
                    break;
            }
            switch (doctorsFilterInfo.AnyPatients)
            {
                case "None":
                    viewModel.Doctors = db.Doctors.OrderBy(d => d.Id).Skip(PageSize * (DoctorsPageNumber - 1)).Take(PageSize);
                    viewModel.DoctorsPageInfo.TotalItems = db.Doctors.Count();
                    break;
                case "With":
                    viewModel.Doctors = db.Doctors.Where(d=>d.Patients.Any()).OrderBy(d => d.Id).Skip(PageSize * (DoctorsPageNumber - 1)).Take(PageSize);
                    viewModel.DoctorsPageInfo.TotalItems = db.Doctors.Where(d => d.Patients.Any()).Count();
                    break;
                case "Without":
                    viewModel.Doctors = db.Doctors.Where(d => !d.Patients.Any()).OrderBy(d => d.Id).Skip(PageSize * (DoctorsPageNumber - 1)).Take(PageSize);
                    viewModel.DoctorsPageInfo.TotalItems = db.Doctors.Where(d => !d.Patients.Any()).Count();
                    break;
                default:
                    break;
            }
            if (DoctorNameFilter != "") { viewModel.Doctors = viewModel.Doctors.Where(d => d.FullName.Contains(DoctorNameFilter)); }
            return View(viewModel);
        }

        [AdminAuthentificationFilter]
        [HttpPost]
        public async Task<ActionResult> SubmitDoctorNameFilter(string DoctorName)
        {
            
            return RedirectToAction("EtoNeVhodDlyaAdmina", new RouteValueDictionary(
    new { controller = "accounts", action = "AdminMaster", DoctorNameFilter = DoctorName }));
        }

        [AdminAuthentificationFilter]
        [HttpPost]
        public async Task<ActionResult> SubmitDoctorsFilter(string AnyPatients)
        {
            
            return RedirectToAction("EtoNeVhodDlyaAdmina", new RouteValueDictionary(
    new { controller = "accounts", action = "AdminMaster", AnyPatients = AnyPatients }));
        }
        [AdminAuthentificationFilter]
        [HttpPost]
        public async Task<ActionResult> SubmitUsersFilter(string AnySessions)
        {
            
            return RedirectToAction("EtoNeVhodDlyaAdmina", new RouteValueDictionary(
    new { controller = "accounts", action = "AdminMaster",AnySessions = AnySessions }));
        
    }
        [AdminAuthentificationFilter]
        [HttpPost]
        public async Task<ActionResult> CreateDoctor(string username,string password,string fullname,string description)
        {
            if (db.Accounts.Where(a => a.Name == username).Count()!=0)
            {
                return RedirectToAction("EtoNeVhodDlyaAdmina");
            }
            Doctor doctor = new Doctor() { FullName = fullname, Description = description };
            Account account = new Account() { AccountType= AccountType.Doctor, Name=username, Password=password };
            db.Accounts.Add(account);
            db.Doctors.Add(doctor);
            account.User = doctor;
            doctor.Account = account;
            db.SaveChanges();

            return RedirectToAction("EtoNeVhodDlyaAdmina");
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
        [ExceptionFilter]
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