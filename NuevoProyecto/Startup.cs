using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CatProject.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace NuevoProyecto
{
    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddSwaggerGen();

            //lee el connection string desde el archivo appsettings.json con el identificador "gatos_db"
            string connectionString = configuration.GetConnectionString("gatos_db");

            //especificamos el uso de SQL SERVER con el contexto GatosContexto
            services.AddDbContext<GatosContexto>(opcion => opcion.UseSqlServer(connectionString));

            //configuramos el tipo de ciclo de vida del objeto (GatosContexto) a inyectar a futuro en los controllers
            services.AddScoped<DbContext, GatosContexto>();

            services.AddCors(options =>
            {
                options.AddPolicy("CrossOriginResourceSharingPolicy",
                builder => builder
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                );
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                    options.RoutePrefix = string.Empty;
                });
            }

            app.UseRouting();

            app.UseCors("CrossOriginResourceSharingPolicy");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
