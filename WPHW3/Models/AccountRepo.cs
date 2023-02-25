using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WPHW3.Models
{
    public class AccountRepo:IAccountRepo
    {
        private AccountDBContext db = new AccountDBContext();
        public IEnumerable<Account> GetAccounts()
        {
            return db.Accounts;
        }
        public Account FindAcccount(int id)
        {
            return db.Accounts.Find(id);
        }
        public void RemoveAccount(int id)
        {
            db.Accounts.Remove(db.Accounts.Find(id));
            db.SaveChanges();
        }
        public void AddAccount(Account account)
        {
            db.Accounts.Add(account);
            db.SaveChanges();
        }
        public void SaveChanges()
        {
            db.SaveChanges();
        }

        public IEnumerable<Session> GetSessions()
        {
            return db.Sessions;
        }
        public Session FindSession(int id)
        {
            return db.Sessions.Find(id);
        }
        public void RemoveSession(int id)
        {
            db.Sessions.Remove(db.Sessions.Find(id));
            db.SaveChanges();
        }
        public void AddSession(Session session)
        {
            db.Sessions.Add(session);
            db.SaveChanges();
        }

        public IEnumerable<User> GetUsers()
        {
            return db.Users;
        }
        public User FindUser(int id)
        {
            return db.Users.Find(id);
        }
        public void RemoveUser(int id)
        {
            db.Users.Remove(db.Users.Find(id));
        }
        public void AddUser(User user)
        {
            db.Users.Add(user);
            db.SaveChanges();
        }

        public IEnumerable<Doctor> GetDoctors()
        {
            return db.Doctors;
        }
        public Doctor FindDoctor(int id)
        {
            return db.Doctors.Find(id);
        }
        public void RemoveDoctor(int id)
        {
            db.Doctors.Remove(FindDoctor(id));
        }
        public void AddDoctor(Doctor doctor)
        {
            db.Doctors.Add(doctor);
        }

        public IEnumerable<Patient> GetPatients()
        {
            return db.Patients;
        }
        public Patient FindPatient(int id)
        {
            return db.Patients.Find(id);
        }
        public void RemovePatient(int id)
        {
            db.Patients.Remove(FindPatient(id));
        }
        public void AddPatient(Patient patient)
        {
            db.Patients.Add(patient);
        }
    }
}