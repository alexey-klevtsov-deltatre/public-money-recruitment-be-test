using VacationRental.Core.Models;

namespace VacationRental.Core.Services
{
    public interface IBookingsService
    {
        BookingViewModel Get(int bookingId);
        ResourceIdViewModel Book(BookingBindingModel model);
        OverlappedBookingViewModel GetOverlappings(RentalViewModel rental);
    }
}