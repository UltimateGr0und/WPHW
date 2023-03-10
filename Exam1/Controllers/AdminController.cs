using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Exam1.Models;
using Exam1.Filters;

namespace Exam1.Controllers
{
    public class AdminController : Controller
    {
        private AccountDBContext db = new AccountDBContext();

        // GET: Admin
        [AdminAuthentificationFilter]
        public async Task<ActionResult> AccountIndex()
        {
            return View(await db.Accounts.ToListAsync());
        }

        // GET: Admin/Details/5
        [AdminAuthentificationFilter]
        public async Task<ActionResult> AccountDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = await db.Accounts.FindAsync(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }

        // GET: Admin/Create
        [AdminAuthentificationFilter]
        public ActionResult AccountCreate()
        {
            return View();
        }

        // POST: Admin/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в разделе https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AdminAuthentificationFilter]
        public async Task<ActionResult> AccountCreate([Bind(Include = "Id,Username,Password,AccountType")] Account account)
        {
            if (ModelState.IsValid)
            {
                db.Accounts.Add(account);
                await db.SaveChangesAsync();
                return RedirectToAction("AccountIndex");
            }

            return View(account);
        }

        // GET: Admin/Edit/5
        [AdminAuthentificationFilter]
        public async Task<ActionResult> AccountEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = await db.Accounts.FindAsync(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }

        // POST: Admin/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в разделе https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AdminAuthentificationFilter]
        public async Task<ActionResult> AccountEdit([Bind(Include = "Id,Username,Password,AccountType")] Account account)
        {
            if (ModelState.IsValid)
            {
                db.Entry(account).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("AccountIndex");
            }
            return View(account);
        }

        // GET: Admin/Delete/5
        [AdminAuthentificationFilter]
        public async Task<ActionResult> AccountDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = await db.Accounts.FindAsync(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }

        // POST: Admin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [AdminAuthentificationFilter]
        public async Task<ActionResult> AccountDeleteConfirmed(int id)
        {
            Account account = await db.Accounts.FindAsync(id);
            db.Accounts.Remove(account);
            await db.SaveChangesAsync();
            return RedirectToAction("AccountIndex");
        }
        [AdminAuthentificationFilter]
        public async Task<ActionResult> ProductIndex()
        {
            return View(await db.ProductInfos.ToListAsync());
        }
        [AdminAuthentificationFilter]

        // GET: ProductInfoes/Details/5
        public async Task<ActionResult> ProductDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductInfo productInfo = await db.ProductInfos.FindAsync(id);
            if (productInfo == null)
            {
                return HttpNotFound();
            }
            return View(productInfo);
        }
        [AdminAuthentificationFilter]

        // GET: ProductInfoes/Create
        public ActionResult ProductCreate()
        {
            return View();
        }

        [AdminAuthentificationFilter]
        // POST: ProductInfoes/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в разделе https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ProductCreate([Bind(Include = "Id,Name,Description,Category,Photo,Price,TotalAmount,IsAuction")] ProductInfo productInfo)
        {
            if (ModelState.IsValid)
            {
                db.ProductInfos.Add(productInfo);
                await db.SaveChangesAsync();
                return RedirectToAction("ProductIndex");
            }

            return View(productInfo);
        }

        // GET: ProductInfoes/Edit/5
        [AdminAuthentificationFilter]
        public async Task<ActionResult> ProductEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductInfo productInfo = await db.ProductInfos.FindAsync(id);
            if (productInfo == null)
            {
                return HttpNotFound();
            }
            return View(productInfo);
        }

        // POST: ProductInfoes/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в разделе https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AdminAuthentificationFilter]
        public async Task<ActionResult> ProductEdit([Bind(Include = "Id,Name,Description,Category,Photo,Price,TotalAmount,IsAuction")] ProductInfo productInfo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(productInfo).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("ProductIndex");
            }
            return View(productInfo);
        }

        // GET: ProductInfoes/Delete/5
        [AdminAuthentificationFilter]
        public async Task<ActionResult> ProductDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductInfo productInfo = await db.ProductInfos.FindAsync(id);
            if (productInfo == null)
            {
                return HttpNotFound();
            }
            return View(productInfo);
        }

        // POST: ProductInfoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [AdminAuthentificationFilter]
        public async Task<ActionResult> ProductDeleteConfirmed(int id)
        {
            ProductInfo productInfo = await db.ProductInfos.FindAsync(id);
            db.ProductInfos.Remove(productInfo);
            await db.SaveChangesAsync();
            return RedirectToAction("ProductIndex");
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
