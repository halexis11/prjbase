using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace prjbase.Models
{
    public partial class ImportarMuestra
    {
        public int Id { get; set; }

        [Required]
        public string departamento { get; set; }
        [Required]
        public string municipio { get; set; }
        [Required]
        public string bodega { get; set; }
        public string focalizacion { get; set; }
        public string remesa { get; set; }
        public DateTime fecha { get; set; }
        public string resultado { get; set; }
    }
}
