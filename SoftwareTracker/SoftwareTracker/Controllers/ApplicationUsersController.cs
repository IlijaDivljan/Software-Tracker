using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using EvidencijaSoftvera_IlijaDivljan.DAL;
using EvidencijaSoftvera_IlijaDivljan.Models;
using EvidencijaSoftvera_IlijaDivljan.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;

namespace EvidencijaSoftvera_IlijaDivljan.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ApplicationUsersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ApplicationUsers
        public ActionResult Index()
        {
            var viewModel = new ApplicationUserRoleViewModel();

            viewModel.AppUsers = db.Users.OrderBy(u => u.LastName).ToList();
            viewModel.UserRoles = db.Roles.OrderBy(r => r.Name).ToList();

            return View(viewModel);
        }

        // GET: ApplicationUsers/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ApplicationUser applicationUser = db.Users.Find(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }

            return View(applicationUser);
        }

        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            var roles = db.Roles.OrderByDescending(r => r.Name).ToList();
            //var selected = roles.Single(r => r.Id == userToUpdate.Roles.Single(a => a.UserId == id).RoleId).Id;
            ViewBag.RoleId = new SelectList(db.Roles, "Name", "Name");
            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model, string roleId)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email, FirstName = model.FirstName, LastName = model.LastName };


                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));

                var result = await userManager.CreateAsync(user, model.Password);

                //var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    //await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    var result2 = await userManager.AddToRoleAsync(user.Id, roleId);
                    ViewBag.RoleId = new SelectList(db.Roles, "Name", "Name");

                    return RedirectToAction("Index", "ApplicationUsers");
                }
                
            }

            // If we got this far, something failed, redisplay form
            var roles = db.Roles.OrderByDescending(r => r.Name).ToList();
            return View(model);
        }

        // GET: ApplicationUsers/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = db.Users.Find(id);
            
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            var roles = db.Roles.ToList();
            var selected = roles.Single(r => r.Id == applicationUser.Roles.Single(a => a.UserId == id).RoleId).Id;
            ViewBag.RoleId = new SelectList(db.Roles, "Id", "Name", selected);
            return View(applicationUser);
        }

        // POST: ApplicationUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "Id,FirstName,LastName,Email,PhoneNumber,UserName")] ApplicationUser applicationUser)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(applicationUser).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(applicationUser);"Id,FirstName,LastName,Email,PhoneNumber,UserName"
        //}

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(string id, string roleId)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //var userToUpdate = db.Users.Single(u => u.Id == id);
            var userToUpdate = db.Users.Include(u => u.Roles).Single(u => u.Id == id);
            string oldRolleId = userToUpdate.Roles.Single(u => u.UserId == id).RoleId;

            if (TryUpdateModel(userToUpdate, "",
                new string[] {"Id", "irstName", "LastName", "Email", "PhoneNumber", "UserName"}))
            {
                try
                {
                    if (oldRolleId != roleId)
                    {
                        //IdentityRole newRolle = db.Roles.Find(roleId);
                        //IdentityRole oldRolle = db.Roles.Find(userToUpdate.Roles.Single(u => u.UserId == id).RoleId);

                        IdentityUserRole oldRole = new IdentityUserRole {RoleId = oldRolleId, UserId = userToUpdate.Id};
                        IdentityUserRole newRole = new IdentityUserRole {RoleId = roleId, UserId = userToUpdate.Id};

                        userToUpdate.Roles.Clear();
                        userToUpdate.Roles.Add(newRole);
                    }
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                catch (DataException)
                {

                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            var roles = db.Roles.ToList();
            var selected = roles.Single(r => r.Id == userToUpdate.Roles.Single(a => a.UserId == id).RoleId).Id;
            ViewBag.RoleId = new SelectList(db.Roles, "Id", "Name", selected);
            return View(userToUpdate);
        }

        // GET: ApplicationUsers/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            ApplicationUser applicationUser = db.Users.Find(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            return View(applicationUser);
        }

        // POST: ApplicationUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            ApplicationUser applicationUser = db.Users.Find(id);

            try
            {
                var roleId = db.Roles.Single(r => r.Name == "Admin");

                if (applicationUser.Roles.First().RoleId == roleId.Id)
                {


                    if (roleId.Users.Count < 2)
                    {
                        ViewBag.LastAdmin = "You cannot delete last Admin user of the application.";
                        return View(applicationUser);
                    }
                }
                else
                {
                    //Ovaj dio koda regulise da kada izbrsemo korisnika aplikacije njegovi registrovani kompijueteri
                    //i evidentirani softver se prenese na administratora.
                    var softwareInstalled =
                        db.InstalledSoftware.Include(s => s.User).Where(s => s.ApplicationUserId == id);

                    foreach (var software in softwareInstalled)
                    {
                        software.ApplicationUserId = User.Identity.GetUserId();
                    }

                    var computersRegistered = db.Computers.Include(c => c.User).Where(c => c.ApplicationUserId == id);

                    foreach (var computer in computersRegistered)
                    {
                        computer.ApplicationUserId = User.Identity.GetUserId();
                    }
                    db.SaveChanges();
                }
            }
            catch (Exception)
            {
                // ignored
            }

            db.Users.Remove(applicationUser);
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
