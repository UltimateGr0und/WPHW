using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace WPHW3.Models
{
    public class DoctorsFilterInfo
    {
        public string AnyPatients { get; set; }= "None";
    }
    public class UsersFilterInfo
    {
        public string AnySessions { get; set; } = "None";
    }
    public class PageInfo
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages
        {
            get { return (int)Math.Ceiling((decimal)TotalItems / PageSize); }
        }

    }
    public class AdminViewModel
    {
        public User CurrentUser { get; set; }
        public IEnumerable<User> Users { get; set; }
        public PageInfo UsersPageInfo { get; set; }
        public IEnumerable<Doctor> Doctors { get; set; }
        public PageInfo DoctorsPageInfo { get; set; }
        public DoctorsFilterInfo doctorsFilterInfo { get; set; }
        public UsersFilterInfo usersFilterInfo { get; set; }
    }
}