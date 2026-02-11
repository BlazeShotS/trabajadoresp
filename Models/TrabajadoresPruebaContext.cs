using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace CRUDTRABAJADORES.Models;

public partial class TrabajadoresPruebaContext : DbContext
{
    public TrabajadoresPruebaContext()
    {
    }

    public TrabajadoresPruebaContext(DbContextOptions<TrabajadoresPruebaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cargo> Cargos { get; set; }

    public virtual DbSet<Trabajador> Trabajadors { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=(local);Database=TrabajadoresPrueba;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cargo>(entity =>
        {
            entity.HasKey(e => e.IdCargo).HasName("PK__Cargo__6C9856255D6C94BB");
        });

        modelBuilder.Entity<Trabajador>(entity =>
        {
            entity.HasKey(e => e.IdTrabajador).HasName("PK__Trabajad__6FAFBCF0788F0EAA");

            entity.HasOne(d => d.IdCargoNavigation).WithMany(p => p.Trabajadors)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cargo");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
