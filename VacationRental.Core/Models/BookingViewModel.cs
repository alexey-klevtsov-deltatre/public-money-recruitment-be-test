using System;

namespace VacationRental.Core.Models
{
    public sealed class BookingViewModel
    {
        public int Id { get; set; }
        public int RentalId { get; set; }
        public DateTime Start { get; set; }
        public int Nights { get; set; }
        public int Unit { get; set; }

        public DateTime End() => Start.Date.AddDays(Nights);
        public DateTime EndWithPreparations(RentalViewModel rental) => End().AddDays(rental.PreparationTimeInDays);
    }
}
