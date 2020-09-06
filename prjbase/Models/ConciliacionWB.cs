using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace prjbase.Models
{
    public class ConciliacionWB
    {
        [Key]
        public int IdProducto { get; set; }
        public string Embalaje { get; set; }
        public string Producto { get; set; }
        public decimal CantidadWB { get; set; }
        public decimal CantidadNR { get; set; }
    }
}
