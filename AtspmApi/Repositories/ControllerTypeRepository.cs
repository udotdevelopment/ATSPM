using System.Collections.Generic;
using System.Linq;
using AtspmApi.Models;

namespace AtspmApi.Repositories
{
    public class ControllerTypeRepository : IControllerTypeRepository
    {
        private readonly Models.AtspmApi db = new Models.AtspmApi();

        public List<ControllerType> GetControllerTypes()
        {
            var list = (from r in db.ControllerType
                select r).ToList();

            return list;
        }
    }
}