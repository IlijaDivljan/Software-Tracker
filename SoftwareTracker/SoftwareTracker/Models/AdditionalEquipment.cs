using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EvidencijaSoftvera_IlijaDivljan.Models
{
    public class AdditionalEquipment
    {
        public int AdditionalEquipmentId { get; set; }
        
        [Required]
        [StringLength(60)]
        public string Name { get; set; }

        [StringLength(150)]
        public string Description { get; set; }


        public virtual ICollection<Computers> Computers { get; set; }
    }

}