using System;

namespace VacationRental.Synchronization.Exceptions
{
    public sealed class LockAcquireException : ApplicationException
    {
        public LockAcquireException(string key) : base($"Failed to acquire the lock {key}.")
        {
        }
    }
}
