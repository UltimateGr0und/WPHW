using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WPHW6.Models;

namespace WPHW6.Controllers
{
    public class QuestsController : Controller
    {
        private IEnumerable<Quest> quests = new List<Quest>{
            new Quest() { Id=1,MaxPlayers = 5, MinPlayers = 1, FearLevel = 3, Email = "fear@gmail.com", DifficultyLevel = 3, Company = "Best quests inc", Address = "Pushki st. 34", Name = "AAAAA Strashno", Phone = "88005553535", Rating = 4.9, Time = 3.5, Description = "Long deeeeeeeeescription blablabla bla blablabla balbalbal balblabl", Logo= "https://images.unsplash.com/photo-1500602763988-7d6c9c1263ca?ixlib=rb-4.0.3&ixid=MnwxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8&auto=format&fit=crop&w=874&q=80", 
            Gallery=new List<string> { "https://images.unsplash.com/photo-1633555690973-b736f84f3c1b?ixlib=rb-4.0.3&ixid=MnwxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8&auto=format&fit=crop&w=387&q=80", "https://images.unsplash.com/photo-1635641398283-ee8f83a6e6f5?ixlib=rb-4.0.3&ixid=MnwxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8&auto=format&fit=crop&w=387&q=80" }
            },
            new Quest() {Id=2, MaxPlayers = 5, MinPlayers = 1, FearLevel = 3, Email = "fear@gmail.com", DifficultyLevel = 3, Company = "Best quests inc", Address = "Pushki st. 34", Name = "AAAAA Strashno", Phone = "88005553535", Rating = 4.9, Time = 3.5, Description = "Long deeeeeeeeescription blablabla bla blablabla balbalbal balblabl", Logo= "https://images.unsplash.com/photo-1500602763988-7d6c9c1263ca?ixlib=rb-4.0.3&ixid=MnwxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8&auto=format&fit=crop&w=874&q=80", 
            Gallery=new List<string> { "https://images.unsplash.com/photo-1633555690973-b736f84f3c1b?ixlib=rb-4.0.3&ixid=MnwxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8&auto=format&fit=crop&w=387&q=80", "https://images.unsplash.com/photo-1635641398283-ee8f83a6e6f5?ixlib=rb-4.0.3&ixid=MnwxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8&auto=format&fit=crop&w=387&q=80" }
            },
            new Quest() {Id=3, MaxPlayers = 5, MinPlayers = 1, FearLevel = 3, Email = "fear@gmail.com", DifficultyLevel = 3, Company = "Best quests inc", Address = "Pushki st. 34", Name = "AAAAA Strashno", Phone = "88005553535", Rating = 4.9, Time = 3.5, Description = "Long deeeeeeeeescription blablabla bla blablabla balbalbal balblabl", Logo= "https://images.unsplash.com/photo-1500602763988-7d6c9c1263ca?ixlib=rb-4.0.3&ixid=MnwxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8&auto=format&fit=crop&w=874&q=80", 
            Gallery=new List<string> { "https://images.unsplash.com/photo-1633555690973-b736f84f3c1b?ixlib=rb-4.0.3&ixid=MnwxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8&auto=format&fit=crop&w=387&q=80", "https://images.unsplash.com/photo-1635641398283-ee8f83a6e6f5?ixlib=rb-4.0.3&ixid=MnwxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8&auto=format&fit=crop&w=387&q=80" }
            }
        };
        // GET: Quests
        public ActionResult Index()
        {
            return View(quests);
        }
        public ActionResult AdminPanel()
        {
            return View(quests);
        }

        public ActionResult DescriptionCard(int id)
        {
            Quest quest = quests.ElementAt(id);
            return View(quest);
        }

        // GET: Quests/Details/5
        public ActionResult Details(int id)
        {
            Quest quest = quests.ElementAt(id);
            return View(quest);
        }

        // GET: Quests/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Quests/Create
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

        // GET: Quests/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Quests/Edit/5
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

        // GET: Quests/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Quests/Delete/5
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
