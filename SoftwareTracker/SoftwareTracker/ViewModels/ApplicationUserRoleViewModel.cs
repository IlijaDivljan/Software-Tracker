using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using EvidencijaSoftvera_IlijaDivljan.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace EvidencijaSoftvera_IlijaDivljan.ViewModels
{
    public class ApplicationUserRoleViewModel
    {
        public IEnumerable<ApplicationUser> AppUsers { get; set; }
        public IEnumerable<IdentityRole> UserRoles { get; set; }
    }
}