using System;
using System.Collections.Generic;
using PrediccionPreciosWeb.Modelos;

namespace PrediccionPreciosWeb.Servicios
{
    public interface IServicioDetalleModelo
    {
        IEnumerable<DetalleModelo> ObtenerDetalles();
    }
}
