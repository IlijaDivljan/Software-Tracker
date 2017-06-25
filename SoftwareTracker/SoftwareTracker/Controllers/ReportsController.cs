using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using EvidencijaSoftvera_IlijaDivljan.DAL;
using EvidencijaSoftvera_IlijaDivljan.Models.Enums;
using Microsoft.Ajax.Utilities;
using PagedList;

namespace EvidencijaSoftvera_IlijaDivljan.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ReportsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        
        // GET: Reports
        public ActionResult Index(int? selectedLicense, int? selectedCategory, string selectedUser, int page=1)
        {
            var model = db.InstalledSoftware
                .Include(s => s.User)
                .Include(s => s.Software)
                .Include(s => s.Computer)
                .OrderBy(s => s.Software.Name).ToList();

            //var model = db.InstalledSoftware
            //    .Where(s => (selectedCategory == null|| s.Software.Category == (SoftwareEnum)selectedCategory)
            //    && (selectedLicense == null || s.Software.License == (LicenseEnum)selectedLicense)
            //    && (string.IsNullOrEmpty(selectedUser) || s.ApplicationUserId == selectedUser))
            //    .Include(s => s.User)
            //    .Include(s => s.Software)
            //    .Include(s => s.Computer)
            //    .OrderBy(s => s.Software.Name).ToPagedList(page, 6);

            if (selectedLicense != null)
            {
                model = model.Where(i => i.Software.License == (LicenseEnum)selectedLicense).ToList();
            }

            if (selectedCategory != null)
            {
                model = model.Where(i => i.Software.Category == (SoftwareEnum)selectedCategory).ToList();
            }

            if (!string.IsNullOrEmpty(selectedUser))
            {
                model = model.Where(i => i.User.Id == selectedUser).ToList();
            }

            Session["selectedLicense"] = selectedLicense;
            Session["selectedCategory"] = selectedCategory;
            Session["selectedUser"] = selectedUser;
            Session["page"] = page;

            if (Request.IsAjaxRequest())
            {
                return PartialView("_ReportsList", model.ToPagedList(page, 6));
            }

            ViewBag.SelectedLicense = Enum.GetValues(typeof(LicenseEnum)).Cast<LicenseEnum>()
                .Select(v => new SelectListItem
                {
                    Text = v.ToString(),
                    Value = ((int)v).ToString(),
                    Selected = ((int)v) == selectedLicense

                }).ToList();

            ViewBag.SelectedCategory = Enum.GetValues(typeof(SoftwareEnum)).Cast<SoftwareEnum>()
                .Select(v => new SelectListItem
                {
                    Text = v.ToString(),
                    Value = ((int)v).ToString(),
                    Selected = ((int)v) == selectedCategory

                }).ToList();

            var users = db.Users.OrderBy(c => c.FirstName).ToList();

            ViewBag.SelectedUser = new SelectList(users, "Id", "FullName", selectedUser);

            return View(model.ToPagedList(page, 6));
        }

        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var installedSoftware = await db.InstalledSoftware.FindAsync(id);

            if (installedSoftware == null)
            {
                return HttpNotFound();
            }

            return View(installedSoftware);
        }
    }
}