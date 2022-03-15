using System;

namespace VacationRental.Core.Models
{
    public interface IBookingModel
    {
        int RentalId { get; set; }
        DateTime Start { get; set; }
        int Nights { get; set; }
    }
}
