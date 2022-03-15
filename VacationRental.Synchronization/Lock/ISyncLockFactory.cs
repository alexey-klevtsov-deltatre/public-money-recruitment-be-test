using System;

namespace VacationRental.Synchronization.Lock
{
    public interface ISyncLockFactory
    {
        SyncLock CreateLock(string key, Exception lockFailureException = null);
    }
}
