using System;
using System.Linq;
using VacationRental.Core.Models;
using VacationRental.Repository;

namespace VacationRental.Core.Services
{
    public sealed class BookingsService : IBookingsService
    {
        private readonly IVacationRepository<BookingViewModel> _bookingRepository;
        private readonly IVacationRepository<RentalViewModel> _rentalRepository;

        public BookingsService(IVacationRepository<BookingViewModel> bookingRepository,
            IVacationRepository<RentalViewModel> rentalRepository)
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

            var rental = _rentalRepository.Get(model.RentalId);
            if (rental == null)
                throw new ApplicationException("Rental not found");

            var key = new ResourceIdViewModel { Id = _bookingRepository.NextId() };

            _bookingRepository.Insert(key.Id, new BookingViewModel
            {
                Id = key.Id,
                Nights = model.Nights,
                RentalId = model.RentalId,
                Start = model.Start.Date,
                Unit = GetFreeRoom(rental, model)
            });

            return key;
        }

        private int GetFreeRoom(RentalViewModel rental, BookingBindingModel newBooking)
        {
            var ocupiedUnits = _bookingRepository
                .Get(booking => booking.RentalId == rental.Id && Intersects(rental, newBooking, booking))
                .Select(booking => booking.Unit).ToHashSet();
            if (ocupiedUnits.Count >= rental.Units)
                throw new ApplicationException("Not available");

            for (var unit = 1; unit <= rental.Units; unit++)
            {
                if (!ocupiedUnits.Contains(unit))
                {
                    return unit;
                }
            }

            throw new ApplicationException("Not available");
        }

        private static bool Intersects(RentalViewModel rental, BookingBindingModel firstBooking,
            BookingViewModel secondBooking)
        {
            var secondBookingEndDate =
                secondBooking.Start.AddDays(secondBooking.Nights).AddDays(rental.PreparationTimeInDays);
            var firstBookingEndDate =
                firstBooking.Start.AddDays(firstBooking.Nights).AddDays(rental.PreparationTimeInDays);

            return secondBooking.Start <= firstBooking.Start.Date && secondBookingEndDate > firstBooking.Start.Date ||
                   secondBooking.Start < firstBookingEndDate && secondBookingEndDate >= firstBookingEndDate ||
                   secondBooking.Start > firstBooking.Start && secondBookingEndDate < firstBookingEndDate;
        }
    }
}
