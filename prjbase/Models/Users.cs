using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace prjbase.Models
{
    // Add profile data for application users by adding properties to the Users class
    public class Users : IdentityUser
    {

        [Display(Name = "Sub Oficina")]
        public int? IdSubOficina { get; set; }
        [Display(Name = "Bodega")]
        public int? IdBodega { get; set; }

        [ForeignKey("IdSubOficina")]
        [InverseProperty("Users")]
        [Display(Name = "Sub Oficina")]
        public virtual SubOficina IdSubOficinaNavigation { get; set; }

        [ForeignKey("IdBodega")]
        [InverseProperty("Users")]
        [Display(Name = "Bodega")]
        public virtual Bodega IdBodegaNavigation { get; set; }
    }
}
