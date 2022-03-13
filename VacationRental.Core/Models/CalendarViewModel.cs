using System.Collections.Generic;

namespace VacationRental.Core.Models
{
    public sealed class CalendarViewModel
    {
        public int RentalId { get; set; }
        public IList<CalendarDateViewModel> Dates { get; set; }
    }
}
