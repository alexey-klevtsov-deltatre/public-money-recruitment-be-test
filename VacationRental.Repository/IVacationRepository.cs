using System.Collections.Generic;

namespace VacationRental.Repository
{
    public interface IVacationRepository<T> where T: class, new()
    {
        T Get(int id);
        IEnumerable<T> Get();
        bool Exists(int id);
        void Insert(int id, T data);
        int NextId();
    }
}
