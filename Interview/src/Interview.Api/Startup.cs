using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Interview.Business.Services;
using Interview.Data;
using Interview.Business;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace Interview.Api
{
    public class Startup
    {
        private IConfiguration Configuration { get; }
        private IWebHostEnvironment Environment { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerDocument();
            

            services.AddScoped<IModelObjectService, ModelObjectService>();
            

            if (Environment.IsProduction())
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                options.EnableSensitiveDataLogging()
                       //.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
                       //.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));
                       .UseMySql(Configuration.GetConnectionString("MySQLConnection")));
            }
            else
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                options.EnableSensitiveDataLogging()
                       //.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));
                       .UseMySql(Configuration.GetConnectionString("MySQLConnection")));

            }
            

            services.AddAutoMapper(new[] { typeof(AutomapperConfigurationProfile).Assembly });

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseOpenApi(x => x.PostProcess = (doc, _) =>
            {
                doc.Info.Title = $"Interview API ({env.EnvironmentName})";
            });
            app.UseSwaggerUi3();

            app.UseCors();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }


   
}
