﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prjbase.Models
{
    public partial class Programacion_input
    {
        public Programacion_input()
        {
            NotaRemision = new HashSet<NotaRemision>();
            ProgramacionCentroEducativo = new HashSet<ProgramacionCentroEducativo>();
            ProgramacionProductoDetalle = new HashSet<ProgramacionProductoDetalle>();
        }

        public int Id { get; set; }

        [Display(Name = "Remesa")]
        public int IdRemesa { get; set; }

        [Display(Name = "Bodega")]
        public int? IdBodega { get; set; }

        [Display(Name = "Focalización")]
        public int IdFocalizacion { get; set; }
        [Column(TypeName = "date")]
        public DateTime Fecha { get; set; }

        [Display(Name = "Activo")]
        [Required]
        [StringLength(1)]
        public string Estado { get; set; }

        [Display(Name = "Fecha de creación")]
        [Column(TypeName = "datetime")]
        public DateTime? FechaCreacion { get; set; }

        [Display(Name = "Fecha de modificación")]

        [StringLength(450)]
        public string UsuarioCreacion { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? FechaModificacion { get; set; }

        [Display(Name = "Usuario modificación")]

        [StringLength(450)]
        public string UsuarioModificacion { get; set; }

        [Display(Name = "Bodega")]
        [ForeignKey("IdBodega")]
        [InverseProperty("Programacion")]
        public virtual Bodega IdBodegaNavigation { get; set; }

        [Display(Name = "Focalización")]
        [ForeignKey("IdFocalizacion")]
        [InverseProperty("Programacion")]
        public virtual Focalizacion IdFocalizacionNavigation { get; set; }

        [Display(Name = "Remesa")]
        [ForeignKey("IdRemesa")]
        [InverseProperty("Programacion")]
        public virtual Remesa IdRemesaNavigation { get; set; }
        [InverseProperty("IdProgramacionNavigation")]
        public virtual ICollection<NotaRemision> NotaRemision { get; set; }
        [Display(Name = "Programación")]
        [InverseProperty("IdProgramacionNavigation")]
        public virtual ICollection<ProgramacionCentroEducativo> ProgramacionCentroEducativo { get; set; }


        [InverseProperty("IdProgramacionNavigation")]
        public virtual ICollection<ProgramacionProductoDetalle> ProgramacionProductoDetalle { get; set; }
    }
}