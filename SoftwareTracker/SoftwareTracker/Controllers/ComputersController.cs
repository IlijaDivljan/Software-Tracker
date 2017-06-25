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
using EvidencijaSoftvera_IlijaDivljan.ViewModels;
using Microsoft.AspNet.Identity;
using PagedList;

namespace EvidencijaSoftvera_IlijaDivljan.Controllers
{
   [Authorize]
    public class ComputersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Computers
        public ActionResult Index(string searchTerm=null, int page = 1)
        {
            var model = db.Computers.OrderBy(r => r.Name)
                .Where(r => searchTerm == null || r.Name.StartsWith(searchTerm))
                //.Take(10)
                .ToPagedList(page, 6);


            if (Request.IsAjaxRequest())
            {
                return PartialView("_Computers", model);
            }

            return View(model);
        }

        public ActionResult Autocomplete(string term)
        {
            var model = db.Computers.Where(r => r.Name.StartsWith(term))
                .Take(10)
                .Select(r => new { label = r.Name });

            return Json(model, JsonRequestBehavior.AllowGet);
        }
        // GET: Computers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Computers computers = db.Computers.Find(id);
            if (computers == null)
            {
                return HttpNotFound();
            }

            return View(computers);
        }

        // GET: Computers/Create
        public ActionResult Create()
        {
            ViewBag.ApplicationUserId = new SelectList(db.Users, "Id", "FirstName");

            var computer = new Computers {AdditionalEquipment = new List<AdditionalEquipment>()};
            PopulateAssignedEquipmentData(computer);

            return View();
        }

        // POST: Computers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name,ComputerType,Manufacturer,Model,SerialNumber,Cpu,Ram,VideoCard,ApplicationUserId")] Computers computers, string[] selectedEquipment)
        {
            try
            {
                if (selectedEquipment != null)
                {
                    computers.AdditionalEquipment = new List<AdditionalEquipment>();
                    foreach (var course in selectedEquipment)
                    {
                        var partToAdd = db.AdditionalEquipment.Find(int.Parse(course));
                        computers.AdditionalEquipment.Add(partToAdd);
                    }
                }
                if (ModelState.IsValid)
                {
                    db.Computers.Add(computers);
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
            }
            catch (DataException)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.\nCheck if Serial Number is unique.");
            }

            ViewBag.ApplicationUserId = new SelectList(db.Users, "Id", "FirstName", computers.ApplicationUserId);
            PopulateAssignedEquipmentData(computers);

            return View(computers);
        }

        // GET: Computers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Computers computers = db.Computers
                .Include(c => c.InstalledSoftware)
                .Single(c => c.ComputersId == id);

            PopulateAssignedEquipmentData(computers);

            if (computers == null)
            {
                return HttpNotFound();
            }

            ViewBag.ApplicationUserId = new SelectList(db.Users, "Id", "FirstName", computers.ApplicationUserId);

            return View(computers);
        }

        private void PopulateAssignedEquipmentData(Computers computer)
        {
            if (db.AdditionalEquipment != null)
            {
                var additionalEq = db.AdditionalEquipment;
                HashSet<int> computersAdditionalEq;
                try
                {
                    computersAdditionalEq =
                    new HashSet<int>(computer.AdditionalEquipment.Select(a => a.AdditionalEquipmentId));
                }
                catch (Exception)
                {
                    computersAdditionalEq = new HashSet<int>();
                    
                }
                
                var viewModel = new List<AssignetComputersEquipment>();

                foreach (var part in additionalEq)
                {
                    viewModel.Add(new AssignetComputersEquipment
                    {
                        AdditionalEquipmentId = part.AdditionalEquipmentId,
                        Title = part.Name,
                        Added = computersAdditionalEq.Contains(part.AdditionalEquipmentId)
                    });
                }
                //var viewModel = additionalEq.Select(part => new AssignetComputersEquipment
                //{
                //    AdditionalEquipmentId = part.AdditionalEquipmentId,
                //    Title = part.Name,
                //    Added = computersAdditionalEq.Contains(part.AdditionalEquipmentId)
                //}).ToList();

                ViewBag.Equipment = viewModel;
            }
        }

        // POST: Computers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "ComputersId,Name,ComputerType,Manufacturer,Model,SerialNumber,Cpu,Ram,VideoCard,ApplicationUserId")] Computers computers)
        public ActionResult Edit(int? id, string[] selectedEquipment)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            var computerToUpdate =
                db.Computers.Include(c => c.AdditionalEquipment).Single(c => c.ComputersId == id);

            if (computerToUpdate.ApplicationUserId != User.Identity.GetUserId() && !User.IsInRole("Admin"))
            {
                return RedirectToAction("Edit", new { id = id });
            }

            if (TryUpdateModel(computerToUpdate, "", new string[]{"Name", "ComputerType", 
                "Manufacturer", "Model", "SerialNumber", "Cpu", "Ram", "VideoCard"}))
            {
                try
                {
                    UpdateAssignedEquipmentData(selectedEquipment, computerToUpdate);
                    //computerToUpdate.ApplicationUserId = User.Identity.GetUserId();

                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                catch (DataException)
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            //if (ModelState.IsValid)
            //{
            //    db.Entry(computers).State = EntityState.Modified;
            //    db.SaveChanges();
            //    return RedirectToAction("Index");
            //}
            ViewBag.ApplicationUserId = new SelectList(db.Users, "Id", "FirstName", computerToUpdate.ApplicationUserId);
            PopulateAssignedEquipmentData(computerToUpdate);

            return View(computerToUpdate);
        }

        private void UpdateAssignedEquipmentData(string[] selectedEquipment, Computers computerToUpdate)
        {
            if (selectedEquipment == null)
            {
                computerToUpdate.AdditionalEquipment = new List<AdditionalEquipment>();
                return;
            }

            var selectedEquipmentHS = new HashSet<string>(selectedEquipment);
            var computersEquipment = new HashSet<int>(computerToUpdate.AdditionalEquipment.Select(a => a.AdditionalEquipmentId));

            foreach (var part in db.AdditionalEquipment)
            {
                if (selectedEquipmentHS.Contains(part.AdditionalEquipmentId.ToString()))
                {
                    if (!computersEquipment.Contains(part.AdditionalEquipmentId))
                    {
                        computerToUpdate.AdditionalEquipment.Add(part);
                    }
                }
                else
                {
                    if (computersEquipment.Contains(part.AdditionalEquipmentId))
                    {
                        computerToUpdate.AdditionalEquipment.Remove(part);
                    }
                }
            }
        }

        // GET: Computers/Delete/5
        public ActionResult Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = "Delete failed. Try again, and if the problem persists see your system administrator.";
            }

            Computers computers = db.Computers.Find(id);
            if (computers == null)
            {
                return HttpNotFound();
            }

            return View(computers);
        }

        // POST: Computers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Computers computers = db.Computers.Find(id);

                if (computers.ApplicationUserId != User.Identity.GetUserId() && !User.IsInRole("Admin"))
                {
                    return RedirectToAction("Delete", new{id = id});
                }

                db.Computers.Remove(computers);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch (DataException)
            {
                return RedirectToAction("Delete", new {id = id, saveChangesError = true});
            }
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
