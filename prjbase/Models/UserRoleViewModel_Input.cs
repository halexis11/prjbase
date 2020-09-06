using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prjbase.Models
{
    public class UserRoleViewModel_Input
    {
        [Required]
        public string UserId { get; set; }

        [Display(Name = "Usuario")]
        public string UserName { get; set; }

        public IEnumerable<string> Roles { get; set; }

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