using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using EvidencijaSoftvera_IlijaDivljan.Models.Enums;

namespace EvidencijaSoftvera_IlijaDivljan.Models
{
    public class Computers
    {
        public int ComputersId { get; set; }

        [Required]
        [StringLength(40, ErrorMessage = "Name of the computer cannot be longer than 40 characters.")]
        public string Name { get; set; }

        [Display(Name = "Category")]
        public ComputerEnum ComputerType { get; set; }

        [Required]
        [StringLength(40, ErrorMessage = "Name of the manufacturer cannot be longer than 40 characters.")]
        public string Manufacturer { get; set; }

        [Required]
        [StringLength(60, ErrorMessage = "Name of the model cannot be longer than 40 characters.")]
        public string Model { get; set; }

        [Required]
        [Display(Name = "Serial Number")]
        [StringLength(20, ErrorMessage = "Serial Number cannot be longer than 20 characters.")]
        [Index(IsUnique = true)]
        public string SerialNumber { get; set; }

        [Display(Name = "CPU")]
        [StringLength(40, ErrorMessage = "CPU cannot be longer than 40 characters.")]
        public string Cpu { get; set; }

        [Display(Name = "RAM")]
        [Range(0, 1024, ErrorMessage = "RAM cannot be smaller than 0 or bigger than 1024GB")]
        public float Ram { get; set; }
        
        [Display(Name = "Video Card")]
        public string VideoCard { get; set; }

        [Required]
        [Display(Name = "User")]
        public string ApplicationUserId { get; set; }

        public virtual ApplicationUser User { get; set; }
        public virtual ICollection<AdditionalEquipment> AdditionalEquipment { get; set; }
        public virtual ICollection<InstalledSoftware> InstalledSoftware { get; set; } 
    }
}