using ANP_Academy.DAL.Models;

namespace ANP_Academy.ViewModel.Contabilidad
{
    public class ContabilidadViewModel
    {
        public IEnumerable<SuscripcionFacturaViewModel> SuscripcionesFacturas { get; set; }
        public decimal TotalGeneral { get; set; }
    }
}
