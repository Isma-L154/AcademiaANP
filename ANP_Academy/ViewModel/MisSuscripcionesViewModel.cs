using ANP_Academy.DAL.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ANP_Academy.ViewModel
{
    public class MisSuscripcionesViewModel
    {
    public List<Solicitudes> SuscripcionesPendientes { get; set; }
    public List<Solicitudes> SuscripcionesActivas { get; set; }
    public List<Solicitudes> SuscripcionesRechazadas { get; set; }
    }
}
