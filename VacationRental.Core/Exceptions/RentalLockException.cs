using System;

namespace VacationRental.Core.Exceptions
{
    public sealed class RentalLockException : ApplicationException
    {
        public RentalLockException(int id): base($"Rental booking is currently being updated {id}.")
        {
        }
    }
}
