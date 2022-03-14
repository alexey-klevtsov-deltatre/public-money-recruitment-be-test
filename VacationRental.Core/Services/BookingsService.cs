using System;
using System.Collections.Generic;
using System.Linq;
using VacationRental.Core.Exceptions;
using VacationRental.Core.Extensions;
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

        public IEnumerable<OverlappedBookingViewModel> GetOverlappings(RentalViewModel rental)
        {
            var bookings = _bookingRepository.Get(booking => booking.RentalId == rental.Id).ToArray();

            for (var checkingBookingIdx = 0; checkingBookingIdx < bookings.Length; checkingBookingIdx++)
            {
                var checkedBooking = bookings[checkingBookingIdx];
                if (checkedBooking.Unit > rental.Units)
                {
                    yield return new OverlappedBookingViewModel { OverlappedBookings = new[] { checkedBooking } };
                }

                for (var bookedIdx = checkingBookingIdx + 1; bookedIdx < bookings.Length; bookedIdx++)
                {
                    var booked = bookings[bookedIdx];
                    if (booked.Unit == checkedBooking.Unit && Intersect(rental,
                        checkedBooking, booked))
                    {
                        yield return new OverlappedBookingViewModel
                            { OverlappedBookings = new[] { checkedBooking, booked } };
                    }
                }
            }
        }

        private int GetFreeRoom(RentalViewModel rental, BookingBindingModel newBooking)
        {
            var ocupiedUnits = _bookingRepository.Get(booking => booking.RentalId == rental.Id && Intersect(rental, newBooking, booking))
                .Select(booking => booking.Unit).ToHashSet();
            if (ocupiedUnits.Count >= rental.Units)
                throw new OverbookingException();

            for (var unit = 1; unit <= rental.Units; unit++)
            {
                if (!ocupiedUnits.Contains(unit))
                {
                    return unit;
                }
            }

            throw new OverbookingException();
        }

        public static bool Intersect(RentalViewModel rental, IBookingModel firstBooking, IBookingModel secondBooking)
        {
            var firstBookingEndDate = firstBooking.EndWithPreparations(rental);
            var secondBookingEndDate = secondBooking.EndWithPreparations(rental);

            return secondBooking.Start <= firstBooking.Start && secondBookingEndDate > firstBooking.Start ||
                   secondBooking.Start < firstBookingEndDate && secondBookingEndDate >= firstBookingEndDate ||
                   secondBooking.Start > firstBooking.Start && secondBookingEndDate < firstBookingEndDate;
        }
    }
}
