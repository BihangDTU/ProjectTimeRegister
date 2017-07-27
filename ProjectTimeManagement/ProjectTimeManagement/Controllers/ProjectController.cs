using ProjectTimeManagement.Context;
using ProjectTimeManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public ActionResult Create(Project project)
        {
            if (ModelState.IsValid)
            {
                db.Projects.Add(project);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(project);
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
