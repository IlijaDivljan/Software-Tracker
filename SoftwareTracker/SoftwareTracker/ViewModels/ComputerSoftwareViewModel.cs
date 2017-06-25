using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EvidencijaSoftvera_IlijaDivljan.Models;
using PagedList;

namespace EvidencijaSoftvera_IlijaDivljan.ViewModels
{
    public class ComputerSoftwareViewModel
    {
        public IPagedList ComputersPagedList { get; set; }
        public IEnumerable<Computers> Computers { get; set; }
        public IEnumerable<InstalledSoftware> Installed { get; set; }
        public IEnumerable<Software> Software { get; set; }
    }
}