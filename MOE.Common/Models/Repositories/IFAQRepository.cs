using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Models.Repositories
{
    public interface IFAQRepository
    {
        List<Models.FAQ> GetAll();
        Models.FAQ GetbyID(int id);
        void Add(Models.FAQ item);
        void Remove(int id);
        void Update(Models.FAQ item);
    }
}
