using ANP_Academy.DAL.Models;

namespace ANP_Academy.ViewModel
{
    public class ContabilidadViewModel
    {
        public IEnumerable<Solicitudes> Solicitudes { get; set; }
        public IEnumerable<Suscripcion>  Suscripciones { get; set; }
    }
}
