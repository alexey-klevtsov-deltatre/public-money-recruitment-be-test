using System;
using System.Collections.Generic;
using System.Linq;
using VacationRental.Core.Exceptions;
using VacationRental.Core.Extensions;
using VacationRental.Core.Models;
using VacationRental.Repository;
using VacationRental.Synchronization.Lock;

namespace VacationRental.Core.Services
{
    public sealed class CalendarService : ICalendarService
    {
        private readonly IVacationRepository<RentalViewModel> _rentalRepository;
        private readonly IVacationRepository<BookingViewModel> _bookingRepository;
        private readonly ISyncLockFactory _syncLockFactory;

        public CalendarService(
            IVacationRepository<RentalViewModel> rentalRepository,
            IVacationRepository<BookingViewModel> bookingRepository,
            ISyncLockFactory syncLockFactory)
        {
            _rentalRepository = rentalRepository;
            _bookingRepository = bookingRepository;
            _syncLockFactory = syncLockFactory;
        }

        public CalendarViewModel Get(int rentalId, DateTime start, int nights)
        {
            if (nights < 0)
                throw new ApplicationException("Nights must be positive");

            var rental = _rentalRepository.Get(rentalId);
            if (rental == null)
                throw new ApplicationException("Rental not found");


            using var syncLock = _syncLockFactory.CreateLock(rental.LockKey(), new RentalLockException(rental.Id));

            return GetBookingCalendar(rental, start, nights);
        }

        private CalendarViewModel GetBookingCalendar(RentalViewModel rental, DateTime start, int nights)
        {
            var result = new CalendarViewModel
            {
                RentalId = rental.Id,
                Dates = new List<CalendarDateViewModel>()
            };

            var rentalBookings = _bookingRepository.Get(booked => booked.RentalId == rental.Id).ToArray();
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
                    var endBookingDate = booking.End();
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
