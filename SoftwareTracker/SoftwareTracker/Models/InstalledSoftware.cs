using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace EvidencijaSoftvera_IlijaDivljan.Models
{
    public class InstalledSoftware
    {
        public int InstalledSoftwareId { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Record Date")]
        public DateTime RecordDate { get; set; }

        [Display(Name = "Computer Name")]
        public int ComputersId { get; set; }

        [Display(Name = "Installed Software")]
        public int SoftwareId { get; set; }

        [Display(Name = "User")]
        public string ApplicationUserId { get; set; }

        public virtual Software Software { get; set; }
        public virtual Computers Computer { get; set; }
        public virtual ApplicationUser User { get; set; }

    }
}
