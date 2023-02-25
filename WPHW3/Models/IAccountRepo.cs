using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPHW3.Models
{
    public interface IAccountRepo
    {
        void SaveChanges();
        IEnumerable<Account> GetAccounts();
        Account FindAcccount(int id);
        void RemoveAccount(int id);
        void AddAccount(Account account);

        IEnumerable<Session> GetSessions();
        Session FindSession(int id);
        void RemoveSession(int id);
        void AddSession(Session session);

        IEnumerable<User> GetUsers();
        User FindUser(int id);
        void RemoveUser(int id);
        void AddUser(User user);

        IEnumerable<Doctor> GetDoctors();
        Doctor FindDoctor(int id);
        void RemoveDoctor(int id);
        void AddDoctor(Doctor doctor);

        IEnumerable<Patient> GetPatients();
        Patient FindPatient(int id);
        void RemovePatient(int id);
        void AddPatient(Patient patient);


    }
}
