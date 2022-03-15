using System.Threading;
using VacationRental.Synchronization.Exceptions;
using VacationRental.Synchronization.Lock;
using Xunit;
using static System.Guid;

namespace VacationRental.Synchronization.Tests
{
    [Collection("Unit")]
    public sealed class SyncLockFactoryTests : IClassFixture<UnitFixture>
    {
        private readonly SyncLockFactory _syncLockFactory;

        public SyncLockFactoryTests(UnitFixture fixture)
        {
            _syncLockFactory = fixture.SyncLockFactory;
        }

        [Fact]
        public void GivenDifferentKeys_WhenLocking_ThenAcquireLock()
        {
            var thread1 = new Thread(() => LongRunningOperationWithLocking(NewGuid().ToString()));

            thread1.Start();
            ShortRunningOperationWithLocking(NewGuid().ToString());
        }

        [Fact]
        public void GivenSameKeys_WhenLocking_ThenReturnsErrorWhenAcquireLock()
        {
            var key = NewGuid().ToString();
            var thread1 = new Thread(() => LongRunningOperationWithLocking(key));

            thread1.Start();
            Thread.Sleep(100);
            Assert.Throws<LockAcquireException>(() => ShortRunningOperationWithLocking(key));
        }

        private void LongRunningOperationWithLocking(string key)
        {
            using var syncLock = _syncLockFactory.CreateLock(key);
            Assert.NotNull(syncLock);
            Thread.Sleep(3000);
        }

        private void ShortRunningOperationWithLocking(string key)
        {
            using var syncLock = _syncLockFactory.CreateLock(key);
            Assert.NotNull(syncLock);
        }
    }
}
