using System;
using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;
using VacationRental.Synchronization.Exceptions;

namespace VacationRental.Synchronization.Lock
{
    public sealed class SyncLock : IDisposable
    {
        private readonly string _key;
        private readonly ConcurrentDictionary<string, SyncLock> _lockStorage;
        private readonly ILogger<SyncLock> _logger;

        internal SyncLock(string key, ConcurrentDictionary<string, SyncLock> lockStorage, ILogger<SyncLock> logger, Exception lockFailureException = null)
        {
            _key = key;
            _lockStorage = lockStorage;
            _logger = logger;
           
            if (!_lockStorage.TryAdd(_key, this))
            {
                throw lockFailureException ?? new LockAcquireException(key);
            }
        }

        public void Dispose()
        {
            if (_lockStorage.TryRemove(_key, out var syncLock) && syncLock != this)
            {
                _logger.LogWarning($"SyncLock was corrupted. Key:{_key}");
            }
        }
    }
}
