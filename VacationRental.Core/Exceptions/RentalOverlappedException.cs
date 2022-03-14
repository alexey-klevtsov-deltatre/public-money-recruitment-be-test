using System;
using VacationRental.Core.Models;

namespace VacationRental.Core.Exceptions
{
    public sealed class RentalOverlappedException : ApplicationException
    {
        public RentalOverlappedException(OverlappedBookingViewModel overlapped):base($"There are overlappings: {overlapped}")
        {
        }
    }
}
