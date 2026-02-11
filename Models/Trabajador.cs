using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CRUDTRABAJADORES.Models;

[Table("Trabajador")]
public partial class Trabajador
{
    [Key]
    public int IdTrabajador { get; set; }

    [StringLength(150)]
    public string Nombres { get; set; } = null!;

    [StringLength(150)]
    public string Apellidos { get; set; } = null!;

    [StringLength(50)]
    public string TipoDocumento { get; set; } = null!;

    [StringLength(8)]
    public string NumeroDocumento { get; set; } = null!;

    [StringLength(10)]
    public string Sexo { get; set; } = null!;

    public DateOnly FechaNacimiento { get; set; }

    public byte[]? Foto { get; set; }

    [StringLength(250)]
    public string? Direccion { get; set; }

    public int IdCargo { get; set; }

    [ForeignKey("IdCargo")]
    [InverseProperty("Trabajadors")]
    public virtual Cargo oCargo { get; set; } = null!; //Aca cambiamos a oCargo la o es del objeto
}
