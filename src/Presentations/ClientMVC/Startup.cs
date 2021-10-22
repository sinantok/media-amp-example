using Caching;
using ClientMVC.Extensions;
using ClientMVC.Helpers;
using ClientMVC.Helpers.Interfaces;
using ClientMVC.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace ClientMVC
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
            services.AddCache(Configuration);
            services.AddConfigurations(Configuration);
            services.AddScoped<ICmsDataService, CmsDataService>();
            services.AddScoped<IAmpHelper, AmpHelper>();
            
            services.Configure<RazorViewEngineOptions>(options =>
            {
                options.ViewLocationExpanders.Add(new AmpViewLocationExpander());
            });

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            //use seriLog
            loggerFactory.AddSerilog();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                   name: "newsDetail",
                   pattern: "amp/{page}",
                   defaults: new { controller = "AmpContent", action = "GetView" });

                endpoints.MapControllerRoute(
                    name: "newsDetail",
                    pattern: "{page}",
                    defaults: new { controller = "Content", action = "GetView" });
            });
        }
    }
}
