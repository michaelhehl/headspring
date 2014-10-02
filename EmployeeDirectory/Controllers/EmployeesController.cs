using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EmployeeDirectory.Models;
using PagedList;

namespace EmployeeDirectory.Controllers
{
    public class EmployeesController : Controller
    {
        private EmployeeDirectoryEntities db = new EmployeeDirectoryEntities();

        // GET: Employees
        public ViewResult Index(string sortOrder, string local, string currentFilter, string searchString, int? page)
        {
            var LocalLst = new List<string>();

            var LocalQry = from d in db.Locations
                           orderby d.locationname
                           select d.locationname;

            LocalLst.AddRange(LocalQry.Distinct());
            ViewBag.local = new SelectList(LocalLst);


            ViewBag.CurrentSort = sortOrder;
            ViewBag.LNameSortParm = String.IsNullOrEmpty(sortOrder) ? "lastname_desc" : "";
            ViewBag.FNameSortParm = sortOrder == "firstname" ? "firstname_desc" : "firstname";
            ViewBag.locationSortParm = sortOrder == "locationname" ? "locationname_desc" : "locationname";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;


            var employees = db.Employees.Include(e => e.Department).Include(e => e.Location).Include(e => e.Title);

            if (!String.IsNullOrEmpty(searchString))
            {
                employees = employees.Where(s => s.lastname.ToUpper().Contains(searchString.ToUpper())
                    || s.firstname.ToUpper().Contains(searchString.ToUpper()));
            }

            if (!string.IsNullOrEmpty(local))
            {
                employees = employees.Where(x => x.Location.locationname == local);
            }
            
            switch (sortOrder)
            {
                case "lastname_desc":
                    employees = employees.OrderByDescending(s => s.lastname);
                    break;
                case "firstname":
                    employees = employees.OrderBy(s => s.firstname);
                    break;
                case "firstname_desc":
                    employees = employees.OrderByDescending(s => s.firstname);
                    break;
                case "locationname_desc":
                    employees = employees.OrderByDescending(s => s.Location.locationname);
                    break;
                case "locationname":
                    employees = employees.OrderBy(s => s.Location.locationname);
                    break;
                default:
                    employees = employees.OrderBy(s => s.lastname);
                    break;
            }

            int pageSize = 50;
            int pageNumber = (page ?? 1);
            return View(employees.ToPagedList(pageNumber, pageSize));

        }

        // GET: Employees/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // GET: Employees/Create
        [Authorize(Roles = "HR")]
        public ActionResult Create()
        {
            ViewBag.departmentID = new SelectList(db.Departments, "departmentID", "departmentname");
            ViewBag.locationID = new SelectList(db.Locations, "locationID", "locationname");
            ViewBag.titleID = new SelectList(db.Titles, "titleID", "titlename");
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "HR")]
        public ActionResult Create([Bind(Include = "employeeID,locationID,roleID,titleID,departmentID,gender,streetaddress,city,state,zipcode,countryfull,emailaddress,telephonenumber,birthday,firstname,middleinitial,lastname,imagelocation")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                db.Employees.Add(employee);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.departmentID = new SelectList(db.Departments, "departmentID", "departmentname", employee.departmentID);
            ViewBag.locationID = new SelectList(db.Locations, "locationID", "locationname", employee.locationID);
            ViewBag.titleID = new SelectList(db.Titles, "titleID", "titlename", employee.titleID);
            return View(employee);
        }

        // GET: Employees/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            ViewBag.departmentID = new SelectList(db.Departments, "departmentID", "departmentname", employee.departmentID);
            ViewBag.locationID = new SelectList(db.Locations, "locationID", "locationname", employee.locationID);
            ViewBag.titleID = new SelectList(db.Titles, "titleID", "titlename", employee.titleID);
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "HR")]
        public ActionResult Edit([Bind(Include = "employeeID,locationID,roleID,titleID,departmentID,gender,streetaddress,city,state,zipcode,countryfull,emailaddress,telephonenumber,birthday,firstname,middleinitial,lastname,imagelocation")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                db.Entry(employee).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.departmentID = new SelectList(db.Departments, "departmentID", "departmentname", employee.departmentID);
            ViewBag.locationID = new SelectList(db.Locations, "locationID", "locationname", employee.locationID);
            ViewBag.titleID = new SelectList(db.Titles, "titleID", "titlename", employee.titleID);
            return View(employee);
        }

        // GET: Employees/Delete/5
        [Authorize(Roles = "HR")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Employee employee = db.Employees.Find(id);
            db.Employees.Remove(employee);
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
