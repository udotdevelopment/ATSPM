using ATSPM.IRepositories;
using ATSPM.Application.Models;
using System.Collections.Generic;
using System.Linq;

namespace ATSPM.Infrastructure.Repositories.EntityFramework
{
    public class ControllerTypeRepository : IControllerTypeRepository
    {
        private readonly MOEContext db;

        public ControllerTypeRepository(MOEContext db)
        {
            this.db = db;
        }

        public List<ControllerType> GetControllerTypes()
        {
            var list = (from r in db.ControllerTypes
                        select r).ToList();

            return list;
        }
    }
}