using Exam1.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Exam1.Controllers
{
    public class CustomerController : Controller
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
        public async Task<ActionResult> Index()
        {
            Account account = RegistratedAccount();
            if (account == null) { return RedirectToAction("Index", "Registration"); }

            return View(account);
        }

        public async Task<ActionResult> Details(int id)
        {
            ProductInfo product = db.ProductInfos.Find(id);
            return View(product);
        }
        [HttpPost]
        public async Task<ActionResult> AddToBasket(int id,int amount)
        {
            Account account = RegistratedAccount();
            if (account == null) { return RedirectToAction("Index", "Registration"); }

            ProductInfo productInfo = db.ProductInfos.Find(id);
            Product product = new Product { Amount = amount, Owner = account, ProductInfo = productInfo };
            productInfo.Products.Add(product);
            db.Products.Add(product);
            db.SaveChanges();            
            return RedirectToAction("Index");
        }
        public async Task<ActionResult> PersonalCabinet()
        {
            Account account = RegistratedAccount();
            if (account == null) { return RedirectToAction("Index", "Registration"); }

            return View(account);
        }
        public async Task<ActionResult> UpdateProductInfo(int id)
        {
            ProductInfo productInfo= db.ProductInfos.Find(id);
            return View(productInfo);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdateProductInfo([Bind(Include ="Id,Description,Price,TotalAmount,Name")]ProductInfo productInfo)
        {
            Account account = RegistratedAccount();
            if (account == null) { return RedirectToAction("Index", "Registration"); }

            if (ModelState.IsValid)
            {
                db.Entry(productInfo).State = EntityState.Modified;
                db.SaveChanges();
            }

            return RedirectToAction("PersonalCabinet");
        }
        public async Task<ActionResult> CreateProductInfo()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateProductInfo([Bind(Include = "Id,Description,Price,TotalAmount,Name")] ProductInfo productInfo)
        {
            Account account = RegistratedAccount();
            if (account == null) { return RedirectToAction("Index", "Registration"); }

            if (ModelState.IsValid)
            {
                db.ProductInfos.Add(productInfo);
                account.ProductsToSell.Add(productInfo);
                productInfo.Seller = account;
                db.SaveChanges();
            }

            return RedirectToAction("PersonalCabinet");
        }
        public async Task<ActionResult> Basket()
        {
            Account account = RegistratedAccount();
            if (account == null) { return RedirectToAction("Index", "Registration"); }
            
            return View(account);
        }
        public async Task<ActionResult> RemoveFromBasket(int id)
        {
            Account account = RegistratedAccount();
            if (account == null) { return RedirectToAction("Index", "Registration"); }

            Product product = db.Products.Find(id);
            account.ProductsToBuy.Remove(product);
            db.SaveChanges();

            return RedirectToAction("Basket");
        }
        public async Task<ActionResult> ConfirmOrder()
        {
            Account account = RegistratedAccount();
            if (account == null) { return RedirectToAction("Index", "Registration"); }

            foreach (var p in account.ProductsToBuy)
            {
                p.ProductInfo.TotalAmount -= p.Amount;
                account.ProductsToBuy.Remove(p);
                db.Products.Remove(p);
            }
            db.SaveChanges();
            return View();
        }

    }
}