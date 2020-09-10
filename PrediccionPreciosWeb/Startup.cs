using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using PrediccionPreciosWeb.Servicios;
using System.IO;

using Microsoft.Extensions.ML;
using PrediccionPreciosWeb.ModelosML;

namespace PrediccionPreciosWeb
{
    public class Startup
    {
        private readonly IWebHostEnvironment _env;

        private static string rutaModelo;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            _env = env;

            rutaModelo = Path.Join(_env.WebRootPath, "data", "MLModel.zip");
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();

            services.AddTransient<IServicioDetalleModelo, ServicioDetalleModelo>(
                (options) =>
                {
                    string ruta = Path.Join(_env.WebRootPath, "data", "carmakerdetails.json");
                    return new ServicioDetalleModelo(ruta);
                });

            services.AddPredictionEnginePool<CarroML, Prediccion>().FromFile(rutaModelo);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
