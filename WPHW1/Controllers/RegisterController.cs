using Microsoft.AspNetCore.Mvc;
using WPHW1.Models;

namespace WPHW1.Controllers
{
    public class RegisterController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Registration()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SubmitRegistration(string FirstName,string LastName,string Password,string Email, string Gender)
        {
            ViewBag.FirstName = FirstName;
            Account account = new Account()
            {
                CreatedDate = DateTime.Now,
                FirstName = FirstName,
                LastName = LastName,
                Password = Password,
                Email = Email,
                Gender = Gender
            };
            

            return View();
        }

    }
}
