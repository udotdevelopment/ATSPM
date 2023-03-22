using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MOE.Common.Models;
using MOE.Common.Models.Repositories;

namespace MOE.CommonTests.Models
{
    public class InMemoryControllerTypeRepository : IControllerTypeRepository
    {
        private InMemoryMOEDatabase _db;
       

        public InMemoryControllerTypeRepository(InMemoryMOEDatabase db)
        {
            this._db = db;
        }

        public InMemoryControllerTypeRepository()
        {
            this._db = new InMemoryMOEDatabase();
        }

        public List<ControllerType> GetControllerTypes()
        {
            var controllerTypes = (from r in _db.ControllerTypes
                select r).ToList();

            return controllerTypes;
        }

        public ControllerType GetControllerTypeByDesc(string desc)
        {
            return _db.ControllerTypes.Find(x => x.Description == desc);
        }
    }
}