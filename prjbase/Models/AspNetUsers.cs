﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prjbase.Models
{
    public partial class AspNetUsers
    {
        public string Id { get; set; }
        [StringLength(256)]
        public string UserName { get; set; }
        [StringLength(256)]
        public string NormalizedUserName { get; set; }
        [StringLength(256)]
        public string Email { get; set; }
        [StringLength(256)]
        public string NormalizedEmail { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string ConcurrencyStamp { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        public int? IdSubOficina { get; set; }
        public int? IdBodega { get; set; }

        [ForeignKey("IdBodega")]
        [InverseProperty("AspNetUsers")]
        public virtual Bodega IdBodegaNavigation { get; set; }
        [ForeignKey("IdSubOficina")]
        [InverseProperty("AspNetUsers")]
        public virtual SubOficina IdSubOficinaNavigation { get; set; }
    }
}