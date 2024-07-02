using System;

namespace ANP_Academy.ViewModel
{
    public class SolicitudesViewModel
    {
        public int IdSolicitud { get; set; }
        public string UserId { get; set; }
        public int IdSuscripcion { get; set; }
        public string NombreSuscripcion { get; set; }
        public byte[] Comprobante { get; set; }
        public DateTime FechaSolicitud { get; set; }
    }

}

