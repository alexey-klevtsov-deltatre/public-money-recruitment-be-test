using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace VacationRental.Repository
{
    public sealed class VacationInMemoryRepository<T> : IVacationRepository<T> where T : class, new()
    {
        private readonly IDictionary<int, T> _storage = new ConcurrentDictionary<int, T>();
        
        public T Get(int id) => !_storage.ContainsKey(id) ? default : _storage[id];

        public IEnumerable<T> Get(Func<T, bool> predicate) => _storage.Values.Where(predicate);

        public IEnumerable<T> Get() => _storage.Values;

        public void Insert(int id, T data)
        {
            if (_storage.ContainsKey(id))
            {
                throw new ArgumentException($"The value already exists. Id:{id}");
            }

            _storage[id] = data;
        }

        public T Update(int id, T data)
        {
            if (!_storage.ContainsKey(id))
            {
                throw new ArgumentException($"There is no entry with id:{id}.");
            }

            _storage[id] = data;

            return _storage[id];
        }

        public int NextId() => _storage.Keys.Count + 1;
    }
}
