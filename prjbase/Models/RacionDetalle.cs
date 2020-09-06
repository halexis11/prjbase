﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prjbase.Models
{
    public partial class RacionDetalle
    {
        public int Id { get; set; }
        [Display(Name = "Ración")]
        public int IdRacion { get; set; }
        [Display(Name = "Producto")]
        public int IdProducto { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        [Display(Name = "Cantidad(en gramos)")]
        public decimal CantidadGramos { get; set; }

        [ForeignKey("IdProducto")]
        [InverseProperty("RacionDetalle")]
        [Display(Name = "Producto")]
        public virtual Producto IdProductoNavigation { get; set; }
        [ForeignKey("IdRacion")]
        [InverseProperty("RacionDetalle")]
        [Display(Name = "Ración")]
        public virtual Racion IdRacionNavigation { get; set; }
    }
}