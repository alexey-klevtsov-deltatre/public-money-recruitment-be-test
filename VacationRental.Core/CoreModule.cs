using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VacationRental.Core.Models;
using VacationRental.Core.Services;
using VacationRental.Repository;
using VacationRental.Synchronization;

namespace VacationRental.Core
{
    public static class CoreModule
    {
        public static IServiceCollection AddVacationRentalCore(this IServiceCollection services, IConfiguration configuration) =>
            services.AddSynchronization(configuration)
                .AddSingleton<IVacationRepository<BookingViewModel>, VacationInMemoryRepository<BookingViewModel>>()
                .AddSingleton<IVacationRepository<RentalViewModel>, VacationInMemoryRepository<RentalViewModel>>()
                .AddSingleton<IBookingsService, BookingsService>().AddSingleton<ICalendarService, CalendarService>()
                .AddSingleton<IRentalsService, RentalsService>();
    }
}
