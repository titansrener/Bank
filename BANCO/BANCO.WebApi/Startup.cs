using BANCO.Data;
using BANCO.Data.Implementation;
using BANCO.Data.Interface;
using BANCO.Manager.Implementation;
using BANCO.Manager.Interface;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.Globalization;
using System.IO;
using System.Linq;

namespace BANCO.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            ambiente = env?.EnvironmentName.Replace("Staging;", "", StringComparison.InvariantCulture)
                        .Replace("Stage;", "", StringComparison.InvariantCulture)
                        .Split(';')
                        .Length > 1 ? env?.EnvironmentName.Split(';')[1] : env?.EnvironmentName.Split(';')[0];
        }

        public IConfiguration Configuration { get; }
        private readonly string ambiente = "";

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddNewtonsoftJson();
            services.AddDbContext<Contexto>(options => options.EnableSensitiveDataLogging().UseSqlServer(Configuration.GetConnectionString("StringConnection")));
            services.AddOptions();

            services.AddSwaggerGen(c =>
            {
                c.CustomOperationIds(e => $"{e.ActionDescriptor.RouteValues["controller"]}_{e.HttpMethod}");
                GetServidorEBase(out string server, out string bd);
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = $"RSP - {ambiente}",
                    Version = "v1",
                    Description = "API de responsabilidades.",
                    License = new OpenApiLicense
                    {
                        Name = $"Servidor BD:{server} Base:{bd}"
                    }
                });
                var filePath = Path.Combine(AppContext.BaseDirectory, "BANCO.WebApi.xml");
                c.IncludeXmlComments(filePath);
            });

            ConfiguraInjecaoDependencia(services);
        }

        private static void ConfiguraInjecaoDependencia(IServiceCollection services)
        {
            services.AddTransient<IContaRepository, ContaRepository>();
            services.AddTransient<IContaManager, ContaManager>();
        }
        private void GetServidorEBase(out string server, out string bd)
        {
            var connection = Configuration.GetConnectionString("StringConnection").Split(';').ToList();
            server = connection.FirstOrDefault(p => p.Contains("Data Source", StringComparison.InvariantCulture)).Split('=')[1];
            bd = connection.FirstOrDefault(p => p.Contains("Initial Catalog", StringComparison.InvariantCulture)).Split('=')[1];
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var supportedCultures = new[] { new CultureInfo("pt-BR") };
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(culture: "pt-BR", uiCulture: "pt-BR"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = string.Empty;
                string swaggerJsonBasePath = string.IsNullOrWhiteSpace(c.RoutePrefix) ? "." : "..";
                c.SwaggerEndpoint($"{swaggerJsonBasePath}/swagger/v1/swagger.json", "API");
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
