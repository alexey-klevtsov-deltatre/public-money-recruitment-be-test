using System.Collections.Generic;
using VacationRental.Core.Models;

namespace VacationRental.Core.Services
{
    public interface IBookingsService
    {
        BookingViewModel Get(int bookingId);
        ResourceIdViewModel Book(BookingBindingModel model);
        IEnumerable<OverlappedBookingViewModel> GetOverlappings(RentalViewModel rental);
    }
}
