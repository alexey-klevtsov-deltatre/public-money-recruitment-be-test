using System;
using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;
using Polly;
using VacationRental.Synchronization.Exceptions;
using VacationRental.Synchronization.Settings;

namespace VacationRental.Synchronization.Lock
{
    public sealed class SyncLockFactory : ISyncLockFactory
    {
        private static readonly ConcurrentDictionary<string, SyncLock> LockStorage = new();
        private readonly ILogger<SyncLock> _logger;
        private readonly ISyncPolicy _retryPolicy;

        public SyncLockFactory(SynchronizationSettings synchronizationSettings, ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<SyncLock>();
            _retryPolicy = Policy.Handle<LockAcquireException>().WaitAndRetry((int)synchronizationSettings.LockMaxRetryCount,
                attempt => TimeSpan.FromSeconds(Math.Pow(1.5, attempt)));
        }

        public SyncLock CreateLock(string key) => _retryPolicy.Execute(() => new SyncLock(key, LockStorage, _logger));
    }
}
