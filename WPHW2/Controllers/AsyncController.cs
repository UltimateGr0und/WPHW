using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WPHW2.Controllers
{
    public class AsyncController : Controller
    {
        // GET: Async
        public ActionResult Index()
        {
            return View();
        }

        // GET: Async/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Async/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Async/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Async/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Async/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Async/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Async/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
