using System;
using System.Collections.Generic;

namespace VacationRental.Repository
{
    public interface IVacationRepository<T> where T: class, new()
    {
        T Get(int id);
        IEnumerable<T> Get(Func<T, bool> predicate);
        IEnumerable<T> Get();
        void Insert(int id, T data);
        T Update(int id, T data);
        int NextId();
    }
}
