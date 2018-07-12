using System.Collections.Generic;

namespace MOE.Common.Models.Repositories
{
    public interface IFAQRepository
    {
        List<FAQ> GetAll();
        FAQ GetbyID(int id);
        void Add(FAQ item);
        void Remove(int id);
        void Update(FAQ item);
    }
}