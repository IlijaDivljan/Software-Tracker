using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using EvidencijaSoftvera_IlijaDivljan.Models.Enums;

namespace EvidencijaSoftvera_IlijaDivljan.Models
{
    public class Software
    {
        public int SoftwareId { get; set; }

        [Required]
        public SoftwareEnum Category { get; set; }

        [Required]
        [StringLength(40, ErrorMessage = "Name of the manufacturer cannot be longer than 40 characters.")]
        public string Manufacturer { get; set; }

        [Required]
        [StringLength(80, ErrorMessage = "Name cannot be longer than 40 characters.")]
        public string Name { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "Version cannot be longer than 40 characters.")]
        public string Version { get; set; }

        [Required]
        public LicenseEnum License { get; set; }

        public virtual ICollection<InstalledSoftware> Installations { get; set; }
    }
}