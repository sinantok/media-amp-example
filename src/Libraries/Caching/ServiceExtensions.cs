using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Caching
{
    public static class ServiceExtensions
    {
        public static void AddCache(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMemoryCache();
            services.AddScoped<ICacheProvider, MemoryCacheProvider>();
        }
    }
}
