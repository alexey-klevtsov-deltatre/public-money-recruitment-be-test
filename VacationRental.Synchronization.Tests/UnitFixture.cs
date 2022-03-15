using Microsoft.Extensions.Logging;
using Moq;
using VacationRental.Synchronization.Lock;
using VacationRental.Synchronization.Settings;
using Xunit;

namespace VacationRental.Synchronization.Tests
{
    [Collection("Unit")]
    public sealed class UnitFixture
    {
        public SyncLockFactory SyncLockFactory { get; }

        public UnitFixture()
        {
            SyncLockFactory = new SyncLockFactory(new SynchronizationSettings { LockMaxRetryCount = 1 },
                new Mock<ILoggerFactory>().Object);
        }
    }
}
