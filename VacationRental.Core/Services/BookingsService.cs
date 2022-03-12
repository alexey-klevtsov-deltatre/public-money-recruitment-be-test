using System;
using System.Linq;
using VacationRental.Core.Model;
using VacationRental.Core.Models;
using VacationRental.Repository;

namespace VacationRental.Core.Services
{
    public sealed class BookingsService : IBookingsService
    {
        private readonly IVacationRepository<BookingViewModel> _bookingRepository;
        private readonly IVacationRepository<RentalViewModel> _rentalRepository;

        public BookingsService(IVacationRepository<BookingViewModel> bookingRepository, IVacationRepository<RentalViewModel> rentalRepository)
        {
            _bookingRepository = bookingRepository;
            _rentalRepository = rentalRepository;
        }

        public BookingViewModel Get(int bookingId)
        {
            var booking = _bookingRepository.Get(bookingId);

            if (booking == null)
                throw new ApplicationException("Booking not found");

            return booking;
        }

        public ResourceIdViewModel Book(BookingBindingModel model)
        {
            if (model.Nights <= 0)
                throw new ApplicationException("Nigts must be positive");
            if (!_rentalRepository.Exists(model.RentalId))
                throw new ApplicationException("Rental not found");

            for (var i = 0; i < model.Nights; i++)
            {
                var count = _bookingRepository.Get().Count(booking =>
                    booking.RentalId == model.RentalId && (booking.Start <= model.Start.Date &&
                                                           booking.Start.AddDays(booking.Nights) > model.Start.Date) ||
                    (booking.Start < model.Start.AddDays(model.Nights) &&
                     booking.Start.AddDays(booking.Nights) >= model.Start.AddDays(model.Nights)) ||
                    (booking.Start > model.Start &&
                     booking.Start.AddDays(booking.Nights) < model.Start.AddDays(model.Nights)));
                if (count >= _rentalRepository.Get(model.RentalId).Units)
                    throw new ApplicationException("Not available");
            }

            var key = new ResourceIdViewModel { Id = _bookingRepository.NextId() };

            _bookingRepository.Insert(key.Id, new BookingViewModel
            {
                Id = key.Id,
                Nights = model.Nights,
                RentalId = model.RentalId,
                Start = model.Start.Date
            });

            return key;
        }
    }
}
