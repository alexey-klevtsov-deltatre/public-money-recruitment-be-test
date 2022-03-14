using System;
using VacationRental.Core.Models;

namespace VacationRental.Core.Extensions
{
    public static class BookingModelExtensions
    {
        public static DateTime End(this IBookingModel booking) => booking.Start.AddDays(booking.Nights);
        public static DateTime EndWithPreparations(this IBookingModel booking, RentalViewModel rental) => booking.End().AddDays(rental.PreparationTimeInDays);
    }
}
