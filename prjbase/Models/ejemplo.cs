﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prjbase.Models
{
    public partial class ejemplo
    {
        public int id { get; set; }
        [Required]
        [StringLength(50)]
        public string nombre { get; set; }
        [Column(TypeName = "date")]
        public DateTime fecha { get; set; }
    }
}