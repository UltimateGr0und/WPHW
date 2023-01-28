using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace WPHW3.Models
{
    public enum WithOut{
        None,
        With,
        Without
    }
    public class DoctorsFilterInfo
    {
        public WithOut AnyPatients { get; set; }= WithOut.None;
    }
    public class UsersFilterInfo
    {
        public WithOut AnySessions { get; set; } = WithOut.None;
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
    }
}