using System;

namespace VacationRental.Core.Models
{
    public sealed class BookingViewModel : IBookingModel
    {
        public int Id { get; set; }
        public int RentalId { get; set; }
        public DateTime Start { get; set; }
        public int Nights { get; set; }
        public int Unit { get; set; }
    }
}
