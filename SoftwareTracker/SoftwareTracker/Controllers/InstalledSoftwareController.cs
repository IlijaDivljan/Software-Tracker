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
using Microsoft.AspNet.Identity;
using PagedList;

namespace EvidencijaSoftvera_IlijaDivljan.Controllers
{
    [Authorize]
    public class InstalledSoftwareController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: InstalledSoftware
        public async Task<ActionResult> Index(int? selectedComputer, int page = 1)
        {
            var computers = await db.Computers.OrderBy(c => c.Name).ToListAsync();
            ViewBag.SelectedComputer = new SelectList(computers, "ComputersId", "Name", selectedComputer);
            int computerId = selectedComputer.GetValueOrDefault();

            //var model =
            //    db.InstalledSoftware.OrderBy(i => i.ComputersId).Include(i => i.Computer)
            //        .Include(i => i.Software)
            //        .Include(i => i.User)
            //        .ToPagedList(page, 8);

            var model = db.InstalledSoftware
                .Where(s => !selectedComputer.HasValue || s.ComputersId == computerId)
                .OrderBy(s => s.ComputersId)
                .Include(s => s.Software)
                .Include(s => s.User)
                .ToPagedList(page, 8);

            Session["selectedComputer"] = selectedComputer;

            if (Request.IsAjaxRequest())
            {
                return PartialView("_InstalledSoftware", model);
            }

            return View(model);
        }

        // GET: InstalledSoftware/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            InstalledSoftware installedSoftware = await db.InstalledSoftware.FindAsync(id);
            if (installedSoftware == null)
            {
                return HttpNotFound();
            }

            return View(installedSoftware);
        }

        // GET: InstalledSoftware/Create
        public ActionResult Create()
        {
            ViewBag.ComputersId = new SelectList(db.Computers, "ComputersId", "Name");
            ViewBag.SoftwareId = new SelectList(db.Software, "SoftwareId", "Name");
            ViewBag.ApplicationUserId = new SelectList(db.Users, "Id", "FirstName");

            return View();
        }

        // POST: InstalledSoftware/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "InstalledSoftwareId,RecordDate,ComputersId,SoftwareId,ApplicationUserId")] InstalledSoftware installedSoftware)
        {
            if (ModelState.IsValid)
            {
                db.InstalledSoftware.Add(installedSoftware);
                await db.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            ViewBag.ComputersId = new SelectList(db.Computers, "ComputersId", "Name", installedSoftware.ComputersId);
            ViewBag.SoftwareId = new SelectList(db.Software, "SoftwareId", "Manufacturer", installedSoftware.SoftwareId);
            ViewBag.ApplicationUserId = new SelectList(db.Users, "Id", "FirstName", installedSoftware.ApplicationUserId);

            return View(installedSoftware);
        }

        // GET: InstalledSoftware/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            InstalledSoftware installedSoftware = await db.InstalledSoftware.FindAsync(id);
            if (installedSoftware == null)
            {
                return HttpNotFound();
            }

            ViewBag.ComputersId = new SelectList(db.Computers, "ComputersId", "Name", installedSoftware.ComputersId);
            ViewBag.SoftwareId = new SelectList(db.Software, "SoftwareId", "Name", installedSoftware.SoftwareId);

            return View(installedSoftware);
        }

        // POST: InstalledSoftware/Edit/5
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditPost(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var computerToUpdate =
                db.InstalledSoftware.Include(i => i.Software)
                    .Include(i => i.Computer)
                    .Include(i => i.User)
                    .Single(i => i.InstalledSoftwareId == id);

            if (computerToUpdate.ApplicationUserId != User.Identity.GetUserId() && !User.IsInRole("Admin"))
            {
                return RedirectToAction("Edit", new { id = id });
            }

            if (TryUpdateModel(computerToUpdate, "",
                new string[] {"InstalledSoftwareId", "RecordDate", "ComputersId", "SoftwareId"}))
            {
                try
                {
                    await db.SaveChangesAsync();

                    return RedirectToAction("Index", new { selectedComputer = Session["selectedComputer"] });
                }
                catch (DataException)
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }

            ViewBag.ComputersId = new SelectList(db.Computers, "ComputersId", "Name", computerToUpdate.ComputersId);
            ViewBag.SoftwareId = new SelectList(db.Software, "SoftwareId", "Name", computerToUpdate.SoftwareId);

            return View(computerToUpdate);
        }

        // GET: InstalledSoftware/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            InstalledSoftware installedSoftware = await db.InstalledSoftware.FindAsync(id);
            if (installedSoftware == null)
            {
                return HttpNotFound();
            }

            return View(installedSoftware);
        }

        // POST: InstalledSoftware/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            InstalledSoftware installedSoftware = await db.InstalledSoftware.FindAsync(id);

            if (installedSoftware.ApplicationUserId != User.Identity.GetUserId() && !User.IsInRole("Admin"))
            {
                return RedirectToAction("Delete", new {id = id });
            }

            db.InstalledSoftware.Remove(installedSoftware);
            await db.SaveChangesAsync();

            return RedirectToAction("Index", new { selectedComputer = Session["selectedComputer"] });
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
