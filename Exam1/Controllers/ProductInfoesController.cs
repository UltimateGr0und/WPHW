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

namespace Exam1.Controllers
{
    public class ProductInfoesController : Controller
    {
        private AccountDBContext db = new AccountDBContext();

        // GET: ProductInfoes
        public async Task<ActionResult> Index()
        {
            return View(await db.ProductInfos.ToListAsync());
        }

        // GET: ProductInfoes/Details/5
        public async Task<ActionResult> Details(int? id)
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

        // GET: ProductInfoes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProductInfoes/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в разделе https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,Description,Category,Photo,Price,TotalAmount,IsAuction")] ProductInfo productInfo)
        {
            if (ModelState.IsValid)
            {
                db.ProductInfos.Add(productInfo);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(productInfo);
        }

        // GET: ProductInfoes/Edit/5
        public async Task<ActionResult> Edit(int? id)
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
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,Description,Category,Photo,Price,TotalAmount,IsAuction")] ProductInfo productInfo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(productInfo).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(productInfo);
        }

        // GET: ProductInfoes/Delete/5
        public async Task<ActionResult> Delete(int? id)
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
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ProductInfo productInfo = await db.ProductInfos.FindAsync(id);
            db.ProductInfos.Remove(productInfo);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
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
