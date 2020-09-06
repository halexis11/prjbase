using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using prjbase.Models;

namespace prjbase.Models
{
    public partial class prjbaseContext : IdentityDbContext<Users, ApplicationRole, string>
    {
        public prjbaseContext()
        {
        }

        public prjbaseContext(DbContextOptions<prjbaseContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ConciliacionWB> ConciliacionWBs { get; set; }
        public virtual DbSet<Bodega> Bodega { get; set; }
        public virtual DbSet<CentroEducativo> CentroEducativo { get; set; }
        public virtual DbSet<Departamento> Departamento { get; set; }
        public virtual DbSet<Distrito> Distrito { get; set; }
        public virtual DbSet<Focalizacion> Focalizacion { get; set; }
        public virtual DbSet<Genero> Genero { get; set; }
        public virtual DbSet<Municipio> Municipio { get; set; }
        public virtual DbSet<Producto> Producto { get; set; }
        public virtual DbSet<Programacion> Programacion { get; set; }
        public virtual DbSet<ProgramacionCentroEducativo> ProgramacionCentroEducativo { get; set; }
        public virtual DbSet<ProgramacionProductoDetalle> ProgramacionProductoDetalle { get; set; }
        public virtual DbSet<Racion> Racion { get; set; }
        public virtual DbSet<RacionDetalle> RacionDetalle { get; set; }
        public virtual DbSet<Remesa> Remesa { get; set; }
        public virtual DbSet<SubOficina> SubOficina { get; set; }
        public virtual DbSet<Zona> Zona { get; set; }
        public virtual DbSet<NotaRemision> NotaRemision { get; set; }
        public virtual DbSet<ImportarMuestra> ImportarMuestra { get; set; }
        public virtual DbSet<Embalaje> Embalaje { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

        public DbSet<prjbase.Models.WayBill> WayBill { get; set; }

        public DbSet<prjbase.Models.WayBillDetalle> WayBillDetalle { get; set; }
    }
}