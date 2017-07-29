using ProjectTimeManagement.Context;
using ProjectTimeManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ProjectTimeManagement.Controllers
{
    public class ProjectController : Controller
    {
        ProjectContext db = new ProjectContext();

        public ActionResult Index()
        {
            return View(db.Projects.ToList());
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Project/Create
        [HttpPost]
        public ActionResult Create(ProjectViewModel model)
        {
            if (ModelState.IsValid)
            {
                Customer c = new Customer();
                c.CustomerName = model.CustomerName;
                c.Email = model.Email;
                c.Phone = model.Phone;
                c.Address = model.Address;
                db.Customers.Add(c);
                db.SaveChanges();

                Project p = new Project();
                p.CreatorName = model.CreatorName;
                p.ProjectName = model.ProjectName;
                p.CreatedTime = model.CreatedTime;
                p.CustomerId = c.CustomerId;
                db.Projects.Add(p);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        // POST: Project/Create
        [HttpPost]
        public ActionResult Register(int? id, TimeTable timeTable)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (ModelState.IsValid)
            {
                TimeTable t = new TimeTable();
                t.ProjectId = id ?? default(int);
                t.RegisterTime = timeTable.RegisterTime;
                t.RegisterName = timeTable.RegisterName;
                t.Hours = timeTable.Hours;

                db.TimeTables.Add(t);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(timeTable);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View((from timeTable in db.TimeTables
                         where timeTable.ProjectId == id
                         select timeTable).ToList());
        }

        // GET: Project/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Project/Edit/5
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

        // GET: Project/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Project/Delete/5
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
