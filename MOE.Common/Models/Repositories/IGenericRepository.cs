using System.Collections.Generic;

namespace MOE.Common.Models.Repositories
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