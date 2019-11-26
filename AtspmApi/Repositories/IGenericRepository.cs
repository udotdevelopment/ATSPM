using System.Collections.Generic;

using AtspmApi.Models;

namespace AtspmApi.Repositories
{
    public interface IGenericRepository<T>
    {
        IEnumerable<T> GetAll();
        void Add(T entity);
        void Delete(T entity);
        void Update(T entity);
        T GetByID(int id);
        void Save();
    }
}