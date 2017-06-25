using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EvidencijaSoftvera_IlijaDivljan.DAL;
using EvidencijaSoftvera_IlijaDivljan.Models;
using PagedList;

namespace EvidencijaSoftvera_IlijaDivljan.Controllers
{
    [Authorize]
    public class AdditionalEquipmentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: AdditionalEquipments
        public ActionResult Index(string searchTerm = null, int page = 1)
        {
            var model = db.AdditionalEquipment.OrderBy(r => r.Name)
                .Where(r => searchTerm == null || r.Name.StartsWith(searchTerm))
                //.Take(10)
                .ToPagedList(page, 10);

            if (Request.IsAjaxRequest())
            {
                return PartialView("_Equipment", model);
            }

            return View(model);
        }

        public ActionResult Autocomplete(string term)
        {
            var model = db.AdditionalEquipment.Where(r => r.Name.StartsWith(term))
                .Take(10)
                .Select(r => new { label = r.Name });

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        // GET: AdditionalEquipments/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AdditionalEquipment additionalEquipment = await db.AdditionalEquipment.FindAsync(id);
            if (additionalEquipment == null)
            {
                return HttpNotFound();
            }
            return View(additionalEquipment);
        }

        // GET: AdditionalEquipments/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdditionalEquipments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "AdditionalEquipmentId,Name,Description")] AdditionalEquipment additionalEquipment)
        {
            if (ModelState.IsValid)
            {
                db.AdditionalEquipment.Add(additionalEquipment);

                await db.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return View(additionalEquipment);
        }

        // GET: AdditionalEquipments/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            AdditionalEquipment additionalEquipment = await db.AdditionalEquipment.FindAsync(id);
            if (additionalEquipment == null)
            {
                return HttpNotFound();
            }

            return View(additionalEquipment);
        }

        // POST: AdditionalEquipments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "AdditionalEquipmentId,Name,Description")] AdditionalEquipment additionalEquipment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(additionalEquipment).State = EntityState.Modified;

                await db.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return View(additionalEquipment);
        }

        // GET: AdditionalEquipments/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            AdditionalEquipment additionalEquipment = await db.AdditionalEquipment.FindAsync(id);
            if (additionalEquipment == null)
            {
                return HttpNotFound();
            }

            return View(additionalEquipment);
        }

        // POST: AdditionalEquipments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            AdditionalEquipment additionalEquipment = await db.AdditionalEquipment.FindAsync(id);

            db.AdditionalEquipment.Remove(additionalEquipment);
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
