using Microsoft.Extensions.DependencyInjection;
using VacationRental.Core.Models;
using VacationRental.Core.Services;
using VacationRental.Repository;

namespace VacationRental.Core
{
    public static class CoreModule
    {
        public static void AddVacationRentalCore(this IServiceCollection services)
        {
            services.AddSingleton<IVacationRepository<BookingViewModel>, VacationInMemoryRepository<BookingViewModel>>();
            services.AddSingleton<IVacationRepository<RentalViewModel>, VacationInMemoryRepository<RentalViewModel>>();
            services.AddSingleton<IBookingsService, BookingsService>();
            services.AddSingleton<ICalendarService, CalendarService>();
            services.AddSingleton<IRentalsService, RentalsService>();
        }
    }
}
