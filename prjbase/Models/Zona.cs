﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prjbase.Models
{
    public partial class Zona
    {
        public Zona()
        {
            CentroEducativo = new HashSet<CentroEducativo>();
            Racion = new HashSet<Racion>();
            Remesa = new HashSet<Remesa>();
        }

        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Nombre { get; set; }

        [InverseProperty("IdZonaNavigation")]
        public virtual ICollection<CentroEducativo> CentroEducativo { get; set; }
        [InverseProperty("IdZonaNavigation")]
        public virtual ICollection<Racion> Racion { get; set; }
        [InverseProperty("IdZonaNavigation")]
        public virtual ICollection<Remesa> Remesa { get; set; }
    }
}