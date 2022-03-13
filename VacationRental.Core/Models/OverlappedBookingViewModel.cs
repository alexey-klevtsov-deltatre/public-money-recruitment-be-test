using System.Collections.Generic;

namespace VacationRental.Core.Models
{
    public sealed class OverlappedBookingViewModel
    {
        public IReadOnlyCollection<int> OverlappedBookings { get; set; }
    }
}
