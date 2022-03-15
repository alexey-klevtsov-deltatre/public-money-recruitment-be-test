using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace VacationRental.Common.Extensions
{
    public static class RegisterExtensions
    {
      public static IServiceCollection RegisterSettings<T>(this IServiceCollection services, IConfiguration configuration)
            where T : class, new()
        {
            var settings = new T();
            configuration?.Bind(settings);

            services.AddSingleton(settings);

            return services;
        }
    }
}
