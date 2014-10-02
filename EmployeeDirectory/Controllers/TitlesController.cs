using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EmployeeDirectory.Models;

namespace EmployeeDirectory.Controllers
{
    public class TitlesController : Controller
    {
        private EmployeeDirectoryEntities db = new EmployeeDirectoryEntities();

        // GET: Titles
        public ActionResult Index()
        {
            return View(db.Titles.ToList());
        }

        // GET: Titles/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Title title = db.Titles.Find(id);
            if (title == null)
            {
                return HttpNotFound();
            }
            return View(title);
        }

        // GET: Titles/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Titles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "HR")]
        public ActionResult Create([Bind(Include = "titleID,titlename")] Title title)
        {
            if (ModelState.IsValid)
            {
                db.Titles.Add(title);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(title);
        }

        // GET: Titles/Edit/5
        [Authorize(Roles = "HR")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Title title = db.Titles.Find(id);
            if (title == null)
            {
                return HttpNotFound();
            }
            return View(title);
        }

        // POST: Titles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "HR")]
        public ActionResult Edit([Bind(Include = "titleID,titlename")] Title title)
        {
            if (ModelState.IsValid)
            {
                db.Entry(title).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(title);
        }

        // GET: Titles/Delete/5
        [Authorize(Roles = "HR")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Title title = db.Titles.Find(id);
            if (title == null)
            {
                return HttpNotFound();
            }
            return View(title);
        }

        // POST: Titles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Title title = db.Titles.Find(id);
            db.Titles.Remove(title);
            db.SaveChanges();
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
