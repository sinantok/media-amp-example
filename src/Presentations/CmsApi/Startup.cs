using Caching;
using CmsApi.Extensions;
using CmsApi.Helpers;
using Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Serilog;
using Services.Concrete;
using Services.Interface;

namespace CmsApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging(x => x.AddSerilog());

            services.AddMongo(Configuration);
            services.AddCache(Configuration);
            services.AddScoped<INewsService, NewsService>();
            services.AddAutoMapper(typeof(MappingProfiles));

            services.AddCors(options =>
            {
                options.AddPolicy("myclients",
                    builder => builder.WithOrigins("https://localhost:6001")
                        .AllowAnyHeader()
                        .AllowAnyMethod());
            });

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "CmsApi", Version = "v1", Description = "CmsApi Application" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //use seriLog
            loggerFactory.AddSerilog();

            //error middleware
            app.UseErrorHandlingMiddleware();

            //open api swagger
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CmsApi v1"));

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseCors("myclients");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
