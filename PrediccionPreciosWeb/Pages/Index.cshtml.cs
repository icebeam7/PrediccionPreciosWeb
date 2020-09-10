using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

using PrediccionPreciosWeb.Modelos;
using PrediccionPreciosWeb.Servicios;
using Microsoft.AspNetCore.Mvc.Rendering;

using Microsoft.Extensions.ML;
using PrediccionPreciosWeb.ModelosML;

namespace PrediccionPreciosWeb.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        private readonly IEnumerable<DetalleModelo> detalle;

        public SelectList ModelosAuto { get; }

        public bool MostrarPrecio { get; private set; } = false;

        [BindProperty]
        public Carro Carro { get; set; }

        [BindProperty]
        public int IdModeloAuto { get; set; }

        public SelectList Years { get; } =
            new SelectList(Enumerable.Range(1930, DateTime.Today.Year - 1929).Reverse());

        private readonly PredictionEnginePool<CarroML, Prediccion> _poolPrediccion;

        public IndexModel(ILogger<IndexModel> logger,
            IServicioDetalleModelo servicio,
            PredictionEnginePool<CarroML, Prediccion> poolPrediccion)
        {
            _logger = logger;
            detalle = servicio.ObtenerDetalles();
            ModelosAuto = new SelectList(detalle, "Id", "Model", default, "Make");

            _poolPrediccion = poolPrediccion;
        }

        public void OnGet()
        {

        }

        public async Task OnPostAsync()
        {
            DetalleModelo modeloSeleccionado = detalle.Where(x => x.Id == IdModeloAuto).FirstOrDefault();

            Carro.Make = modeloSeleccionado.Make;
            Carro.Model = modeloSeleccionado.Model;

            CarroML carroML = new CarroML()
            {
                Year = Carro.Year,
                Mileage = Carro.Mileage,
                Make = Carro.Make,
                Model = Carro.Model
            };

            Prediccion prediccion = _poolPrediccion.Predict(carroML);
            Carro.Price = prediccion.Score;

            MostrarPrecio = true;
        }
    }
}
