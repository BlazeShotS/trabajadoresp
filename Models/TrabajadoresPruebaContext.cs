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
    {
    }



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cargo>(entity =>
        {
            entity.HasKey(e => e.IdCargo).HasName("PK__Cargo__6C9856255D6C94BB");
        });

        modelBuilder.Entity<Trabajador>(entity =>
        {
            entity.HasKey(e => e.IdTrabajador).HasName("PK__Trabajad__6FAFBCF0788F0EAA");

            entity.HasOne(d => d.oCargo).WithMany(p => p.Trabajadors)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cargo");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
