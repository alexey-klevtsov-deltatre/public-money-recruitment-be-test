using System;
using System.Collections.Generic;

namespace VacationRental.Core.Models
{
    public sealed class CalendarDateViewModel
    {
        public DateTime Date { get; set; }
        public IList<CalendarBookingViewModel> Bookings { get; set; }
        public IList<PreparationViewModel> PreparationTimes { get; set; }
    }
}
