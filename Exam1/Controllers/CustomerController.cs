using Exam1.Models;
using Exam1.Filters;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.IO;
using System.Collections;

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
        private bool SearchProduct(ProductInfo product,string SearchRequest)
        {
            if (product.Name.ToLower().Replace(" ", String.Empty).Contains(SearchRequest) ||
                product.Description.ToLower().Replace(" ", String.Empty).Contains(SearchRequest) ||
                product.Seller.Username.ToLower().Replace(" ", String.Empty).Contains(SearchRequest))
            {
                return true;
            }return false;
        }
        public async Task<ActionResult> Index(int Page=1, string Category = "all", string SearchRequest="")
        {
            SearchRequest = SearchRequest.ToLower().Replace(" ",String.Empty);
            var res = db.ProductInfos.Where(p=>p.Seller!=null).Where(p=>p.Category==Category||Category=="all").Where(product => product.Name.ToLower().Replace(" ", String.Empty).Contains(SearchRequest) ||
                product.Description.ToLower().Replace(" ", String.Empty).Contains(SearchRequest) ||
                product.Seller.Username.ToLower().Replace(" ", String.Empty).Contains(SearchRequest));

            Account account = RegistratedAccount();
            PageInfo pageInfo = new PageInfo() { PageNumber = Page, PageSize=10, TotalItems=res.Count() };
            MarketModelView model = new MarketModelView() { User = account,Category=Category, PageInfo = pageInfo, Categories=db.Categories.Select(c=>c.Name).ToList() };
            model.products = res.OrderBy(p=>p.Id).Skip(10 * (Page-1)).Take(10).ToList();

            return View(model);
        }

        public async Task<ActionResult> Details(int id)
        {
            ProductInfo product = db.ProductInfos.Find(id);
            ViewBag.Comments = product.Comments.ToList();
            ViewBag.User = RegistratedAccount();
            return View(product);
        }
        
        [HttpPost]
        [AuthetificationFilter]
        public async Task<ActionResult> AddApplicant(int id,int price)
        {
            Account account = RegistratedAccount();

            ProductInfo productInfo = db.ProductInfos.Find(id);
            if (productInfo.Price < price)
            {
                productInfo.Price = price;
                if (!productInfo.Applicants.Contains(account))
                {
                    productInfo.Applicants.Add(account);
                    account.AuctionLots.Add(productInfo);
                }

                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        
        [HttpPost]
        [AuthetificationFilter]
        public async Task<ActionResult> AddToBasket(int id,int amount)
        {
            Account account = RegistratedAccount();

            ProductInfo productInfo = db.ProductInfos.Find(id);
            if (productInfo.TotalAmount<amount)
            {
                RedirectToAction("Index");
            }
            Product product = new Product { Amount = amount, Owner = account, ProductInfo = productInfo };
            productInfo.Products.Add(product);
            db.Products.Add(product);
            db.SaveChanges();            
            return RedirectToAction("Index");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthetificationFilter]
        public async Task<ActionResult> AddComment(string text,int id)
        {
            Account account = RegistratedAccount();
            Comment comment = new Comment { Text = text };
            ProductInfo productInfo = db.ProductInfos.Find(id);
            db.Comments.Add(comment);
            db.SaveChanges();
            productInfo.Comments.Add(comment);
            comment.Product = productInfo;
            db.SaveChanges();
            account.Comments.Add(comment);
            comment.Account = account;
            db.SaveChanges();
            return RedirectToAction("Details", new RouteValueDictionary(
    new { controller = "Customer", action = "Details", id = comment.Product.Id }));
        }
        [AuthetificationFilter]
        public async Task<ActionResult> PersonalCabinet()
        {
            Account account = RegistratedAccount();
            ViewBag.Categories = db.Categories.Select(p => p.Name).ToArray();
            List<int> cRawValues = new List<int>();
            foreach (var c in db.Categories)
            {
                cRawValues.Add(account.ProductsToSell.Where(p=>p.Category==c.Name).Count());
            }
            List<int> cValues = new List<int>();
            foreach (var v in cRawValues)
            {
                cValues.Add((int)((double)v / (double)cRawValues.Max() * 270.0));
            }
            ViewBag.cValues= cValues.ToArray();

            return View(account);
        }
        [AuthetificationFilter]
        public async Task<ActionResult> PersonalCabinetAuctions()
        {
            Account account = RegistratedAccount();

            return View(account);
        }
        [AuthetificationFilter]
        public async Task<ActionResult> PersonalCabinetBets()
        {
            Account account = RegistratedAccount();

            return View(account);
        }

        [AuthetificationFilter]
        public async Task<ActionResult> DeleteApplicant(int id)
        {
            Account account = RegistratedAccount();
            ProductInfo product = db.ProductInfos.Find(id);
            account.AuctionLots.Remove(product);
            product.Applicants.Remove(account);
            db.SaveChanges();
            return RedirectToAction("PersonalCabinet");
        }
        [AuthetificationFilter]
        public async Task<ActionResult> FinishAuction(int id)
        {
            Account account = RegistratedAccount();
            ProductInfo product = db.ProductInfos.Find(id);
            List<Account> accounts = product.Applicants.ToList();
            foreach (var a in accounts)
            {
                a.AuctionLots.Remove(product);
            }
            account.ProductsToSell.Remove(product);
            db.ProductInfos.Remove(product);
            db.SaveChanges();
            return RedirectToAction("PersonalCabinetAuctions");
        }
        [AuthetificationFilter]
        public async Task<ActionResult> UpdateProductInfo(int id)
        {
            ViewBag.Categories = new SelectList(db.Categories, "Id", "Name");
            Account account = RegistratedAccount();
            if (!account.ProductsToSell.Where(p=>p.Id==id).Any())
            {
                return RedirectToAction("Index");
            }
            ProductInfo productInfo = db.ProductInfos.Find(id);
            return View(productInfo);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthetificationFilter]
        public async Task<ActionResult> UpdateProductInfo([Bind(Include = "Id,Description,Photo,Price,TotalAmount,Name,Category")] ProductInfo productInfo)
        {
            Account account = RegistratedAccount();
            if (!account.ProductsToSell.Where(p => p.Id == productInfo.Id).Any())
            {
                return RedirectToAction("Index");
            }

            ProductInfo current = db.ProductInfos.Find(productInfo.Id);
            current.Price = productInfo.Price;
            current.Name = productInfo.Name;
            current.Category = productInfo.Category;
            current.Description = productInfo.Description;
            current.TotalAmount = productInfo.TotalAmount;
            current.Photo = productInfo.Photo;
            db.SaveChanges();
            //if (ModelState.IsValid)
            //{
            //    db.Entry(productInfo).State = EntityState.Modified;
            //    db.SaveChanges();
            //}

            return RedirectToAction("PersonalCabinet");
        }
        [AuthetificationFilter]
        public async Task<ActionResult> DeleteProductInfo(int id)
        {
            RegistratedAccount().ProductsToSell.Remove(db.ProductInfos.Find(id));
            db.ProductInfos.Find(id).Seller=null;
            db.SaveChanges();
            return RedirectToAction("PersonalCabinet");
        }
        [AuthetificationFilter]
        public async Task<ActionResult> CreateProductInfo()
        {
            ViewBag.Categories = new SelectList(db.Categories, "Name", "Name");
            ViewBag.Statuses = new SelectList(new List<string> {"product","auction"});
            return View();
        }
        [HttpPost]
        [AuthetificationFilter]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateProductInfo([Bind(Include = "Id,Description,Price,TotalAmount,Name,Category,IsAuction,Photo")] ProductInfo productInfo)
        {
            Account account = RegistratedAccount();

            if (ModelState.IsValid)
            {
                productInfo.Photo = productInfo.Photo == null ? "https://whey.kz/wp-content/uploads/2020/11/placeholder.png" : productInfo.Photo;
                db.ProductInfos.Add(productInfo);
                account.ProductsToSell.Add(productInfo);
                productInfo.Seller = account;
                db.SaveChanges();
            }

            return RedirectToAction("PersonalCabinet");
        }

        [AuthetificationFilter]
        public async Task<ActionResult> AddProductPhoto(int id)
        {
            Account account = RegistratedAccount();
            ProductInfo product = await db.ProductInfos.FindAsync(id);
            if (account.ProductsToSell.Contains(product))
            {
                return View(product);
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        [AuthetificationFilter]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddProductPhoto(HttpPostedFileBase file,int id)
        {

            Account account = RegistratedAccount();
            ProductInfo product = await db.ProductInfos.FindAsync(id);
            if (account.ProductsToSell.Contains(product))
            {
                MemoryStream target = new MemoryStream();
                file.InputStream.CopyTo(target);
                byte[] data = target.ToArray();
                product.Photo = $"data:{file.ContentType};base64,{Convert.ToBase64String(data)}";
                await db.SaveChangesAsync();
            }
            return RedirectToAction("PersonalCabinet");
        }
        [AuthetificationFilter]
        public async Task<ActionResult> Basket()
        {
            Account account = RegistratedAccount();
            
            return View(account);
        }
        [AuthetificationFilter]
        public async Task<ActionResult> RemoveFromBasket(int id)
        {
            Account account = RegistratedAccount();

            Product product = db.Products.Find(id);
            account.ProductsToBuy.Remove(product);
            db.SaveChanges();

            return RedirectToAction("Basket");
        }
        [AuthetificationFilter]
        public async Task<ActionResult> ConfirmOrder()
        {
            Account account = RegistratedAccount();

            foreach (var p in account.ProductsToBuy)
            {
                p.ProductInfo.TotalAmount -= p.Amount;
            }
            db.Products.RemoveRange(account.ProductsToBuy);
            db.SaveChanges();
            return View();
        }

    }
}