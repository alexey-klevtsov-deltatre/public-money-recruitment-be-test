using System;
using System.Collections.Generic;

namespace VacationRental.Repository
{
    public sealed class VacationInMemoryRepository<T> : IVacationRepository<T> where T : class, new()
    {
        private readonly IDictionary<int, T> _storage = new Dictionary<int, T>();
        
        public T Get(int id) => !_storage.ContainsKey(id) ? default : _storage[id];

        public IEnumerable<T> Get() => _storage.Values;

        public bool Exists(int id) => _storage.ContainsKey(id);

        public void Insert(int id, T data)
        {
            if (_storage.ContainsKey(id))
            {
                throw new ArgumentException($"The value already exists. Id:{id}");
            }

            _storage[id] = data;
        }

        public int NextId() => _storage.Keys.Count + 1;
    }
}