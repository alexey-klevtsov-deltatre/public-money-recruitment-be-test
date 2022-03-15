using System.Collections.Generic;
using System.Linq;

namespace VacationRental.Core.Models
{
    public sealed class OverlappedBookingViewModel
    {
        public IReadOnlyCollection<BookingViewModel> OverlappedBookings { get; set; }

        public override string ToString() => string.Concat(OverlappedBookings.Select(ob => $"Id:{ob.Id},Unit:{ob.Unit}"), " ");
    }
}
