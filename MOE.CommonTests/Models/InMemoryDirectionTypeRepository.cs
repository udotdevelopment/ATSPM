using System.Collections.Generic;
using System.Linq;
using MOE.Common.Models;
using MOE.Common.Models.Repositories;

namespace MOE.CommonTests.Models
{

    public class InMemoryDirectionTypeRepository : IDirectionTypeRepository
    {
        private InMemoryMOEDatabase _MOE;

        public InMemoryDirectionTypeRepository()
        {
            _MOE = new InMemoryMOEDatabase();
            
        }

        public InMemoryDirectionTypeRepository(InMemoryMOEDatabase MOE)
        {
            _MOE = MOE;

        }
        public List<DirectionType> GetAllDirections()
        {
            var dirs = (from r in _MOE.DirectionTypes
                       select r).ToList();

            return dirs;
        }

        public DirectionType GetDirectionByID(int directionID)
        {
            var dirs = (from r in _MOE.DirectionTypes
                        where r.DirectionTypeID == directionID
                        select r).FirstOrDefault();

            return dirs;
        }

        public List<System.Web.Mvc.SelectListItem> GetSelectList()
        {
            List<System.Web.Mvc.SelectListItem> list =
                new List<System.Web.Mvc.SelectListItem>();
            foreach (DirectionType d in _MOE.DirectionTypes.OrderBy(d => d.DisplayOrder))
            {
                list.Add(new System.Web.Mvc.SelectListItem { Value = d.DirectionTypeID.ToString(), Text = d.Description });
            }
            return list;
        }
    }
}