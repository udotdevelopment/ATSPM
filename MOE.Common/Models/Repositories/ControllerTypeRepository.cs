using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace MOE.Common.Models.Repositories
{
    public class ControllerTypeRepository : IControllerTypeRepository
    {
        private readonly SPM db = new SPM();

        public List<ControllerType> GetControllerTypes()
        {
            var list = (from r in db.ControllerType
                select r).ToList();

            return list;
        }

        public ControllerType GetControllerTypeByDesc(string desc)
        {
            return db.ControllerType.FirstOrDefault(x => x.Description == desc);
        }
    }
}