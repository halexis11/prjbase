﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prjbase.Models
{
    public partial class Municipio
    {
        public Municipio()
        {
            Bodega = new HashSet<Bodega>();
            CentroEducativo = new HashSet<CentroEducativo>();
            Programacion = new HashSet<Programacion>();
        }

        public int Id { get; set; }
        [Required]
        [StringLength(2)]
        public string Codigo { get; set; }
        [Required]
        [StringLength(80)]
        public string Nombre { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int DepartamentoId { get; set; }

        [ForeignKey("DepartamentoId")]
        [InverseProperty("Municipio")]
        public virtual Departamento Departamento { get; set; }
        [InverseProperty("IdMunicipioNavigation")]
        public virtual ICollection<Bodega> Bodega { get; set; }
        [InverseProperty("IdMunicipioNavigation")]
        public virtual ICollection<CentroEducativo> CentroEducativo { get; set; }
        [InverseProperty("IdMunicipioNavigation")]
        public virtual ICollection<Programacion> Programacion { get; set; }
    }
}