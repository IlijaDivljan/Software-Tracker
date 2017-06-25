using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EvidencijaSoftvera_IlijaDivljan.DAL;
using EvidencijaSoftvera_IlijaDivljan.Models;
using PagedList;

namespace EvidencijaSoftvera_IlijaDivljan.Controllers
{
    [Authorize]
    public class SoftwareController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Software
        public ActionResult Index(string searchTerm = null, int page = 1)
        {
            var model = db.Software.OrderBy(r => r.Name)
                .Where(r => searchTerm == null || r.Name.StartsWith(searchTerm))
                //.Take(10)
                .ToPagedList(page, 10);


            if (Request.IsAjaxRequest())
            {
                return PartialView("_Software", model);
            }

            return View(model);
        }

        public ActionResult Autocomplete(string term)
        {
            var model = db.Software.Where(r => r.Name.StartsWith(term))
                .Take(10)
                .Select(r => new { label = r.Name });

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        // GET: Software/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Software software = db.Software.Find(id);

            if (software == null)
            {
                return HttpNotFound();
            }

            return View(software);
        }

        // GET: Software/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Software/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SoftwareId,Category,Manufacturer,Name,Version,License")] Software software)
        {
            if (ModelState.IsValid)
            {
                db.Software.Add(software);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(software);
        }

        // GET: Software/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Software software = db.Software.Find(id);
            if (software == null)
            {
                return HttpNotFound();
            }

            return View(software);
        }

        // POST: Software/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SoftwareId,Category,Manufacturer,Name,Version,License")] Software software)
        {
            if (ModelState.IsValid)
            {
                db.Entry(software).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(software);
        }

        // GET: Software/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Software software = db.Software.Find(id);
            if (software == null)
            {
                return HttpNotFound();
            }

            return View(software);
        }

        // POST: Software/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Software software = db.Software.Find(id);

            db.Software.Remove(software);
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
