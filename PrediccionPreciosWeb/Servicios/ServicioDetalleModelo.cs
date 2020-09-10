using System.IO;
using System.Text.Json;
using System.Collections.Generic;
using PrediccionPreciosWeb.Modelos;

namespace PrediccionPreciosWeb.Servicios
{
    public class ServicioDetalleModelo : IServicioDetalleModelo
    {
        private readonly IEnumerable<DetalleModelo> detalles;

        public ServicioDetalleModelo(string rutaArchivo)
        {
            string datos = File.ReadAllText(rutaArchivo);
            detalles = JsonSerializer.Deserialize<IEnumerable<DetalleModelo>>(datos);
        }

        public IEnumerable<DetalleModelo> ObtenerDetalles() => detalles;
    }
}
