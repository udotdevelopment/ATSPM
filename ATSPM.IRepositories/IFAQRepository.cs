using ATSPM.Application.Models;
using System.Collections.Generic;

namespace ATSPM.IRepositories
{
    public interface IFAQRepository
    {
        List<Faq> GetAll();
        Faq GetbyID(int id);
        void Add(Faq item);
        void Remove(int id);
        void Update(Faq item);
    }
}