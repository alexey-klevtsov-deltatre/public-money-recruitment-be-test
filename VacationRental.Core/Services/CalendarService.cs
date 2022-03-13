using System;
using System.Collections.Generic;
using System.Linq;
using VacationRental.Core.Models;
using VacationRental.Repository;

namespace VacationRental.Core.Services
{
    public sealed class CalendarService : ICalendarService
    {
        private readonly IVacationRepository<RentalViewModel> _rentalRepository;
        private readonly IVacationRepository<BookingViewModel> _bookingRepository;

        public CalendarService(
            IVacationRepository<RentalViewModel> rentalRepository,
            IVacationRepository<BookingViewModel> bookingRepository)
        {
            _rentalRepository = rentalRepository;
            _bookingRepository = bookingRepository;
        }

        public CalendarViewModel Get(int rentalId, DateTime start, int nights)
        {
            if (nights < 0)
                throw new ApplicationException("Nights must be positive");

            var rental = _rentalRepository.Get(rentalId);
            if (rental == null)
                throw new ApplicationException("Rental not found");

            var result = new CalendarViewModel
            {
                RentalId = rentalId,
                Dates = new List<CalendarDateViewModel>()
            };

            var rentalBookings = _bookingRepository.Get(booked => booked.RentalId == rentalId).ToArray();
            for (var i = 0; i < nights; i++)
            {
                var date = new CalendarDateViewModel
                {
                    Date = start.Date.AddDays(i),
                    Bookings = new List<CalendarBookingViewModel>(),
                    PreparationTimes = new List<PreparationViewModel>()
                };

                foreach (var booking in rentalBookings)
                {
                    var endBookingDate = booking.Start.AddDays(booking.Nights);
                    if (booking.Start <= date.Date && endBookingDate > date.Date)
                    {
                        date.Bookings.Add(new CalendarBookingViewModel { Id = booking.Id, Unit = booking.Unit });
                    }
                    if (endBookingDate <= date.Date && endBookingDate.AddDays(rental.PreparationTimeInDays) > date.Date)
                    {
                        date.PreparationTimes.Add(new PreparationViewModel { Unit = booking.Unit });
                    }
                }

                result.Dates.Add(date);
            }

            return result;
        }
    }
}
