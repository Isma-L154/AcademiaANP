using System;
using System.Collections.Generic;

namespace AcademiaANP.DAL.Models;

public partial class Ubicacion
{
    public int IdUbicacion { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Inventario> Inventarios { get; set; } = new List<Inventario>();
}
