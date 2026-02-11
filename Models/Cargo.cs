using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CRUDTRABAJADORES.Models;

[Table("Cargo")]
public partial class Cargo
{
    [Key]
    public int IdCargo { get; set; }

    [StringLength(100)]
    public string Tipo { get; set; } = null!;

    [InverseProperty("IdCargoNavigation")]
    public virtual ICollection<Trabajador> Trabajadors { get; set; } = new List<Trabajador>();
}
