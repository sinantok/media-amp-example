using ClientMVC.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ClientMVC.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddConfigurations(this IServiceCollection services, IConfiguration Configuration)
        {
            var cms = new CmsConfig();
            Configuration.GetSection("CmsApiConfig").Bind(cms);
            services.AddSingleton(cms);
            return services;
        }
    }
}
