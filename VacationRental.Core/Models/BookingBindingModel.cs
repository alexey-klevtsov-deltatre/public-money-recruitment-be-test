using System;

namespace VacationRental.Core.Models
{
    public sealed class BookingBindingModel
    {
        private DateTime _startIgnoreTime;

        public int RentalId { get; set; }

        public DateTime Start
        {
            get => _startIgnoreTime;
            set => _startIgnoreTime = value.Date;
        }

        public int Nights { get; set; }

        public DateTime End() => Start.Date.AddDays(Nights);
        public DateTime EndWithPreparations(RentalViewModel rental) => End().AddDays(rental.PreparationTimeInDays);
    }
}
