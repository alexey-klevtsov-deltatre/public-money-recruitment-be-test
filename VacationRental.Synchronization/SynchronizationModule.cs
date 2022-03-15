using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VacationRental.Common.Extensions;
using VacationRental.Synchronization.Lock;
using VacationRental.Synchronization.Settings;

namespace VacationRental.Synchronization
{
    public static class SynchronizationModule
    {
        public static IServiceCollection
            AddSynchronization(this IServiceCollection services, IConfiguration configuration) => services
            .RegisterSettings<SynchronizationSettings>(configuration.GetSection("Synchronization_Settings"))
            .AddSingleton<ISyncLockFactory, SyncLockFactory>();
    }
}
