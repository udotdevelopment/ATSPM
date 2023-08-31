using System.Collections.Generic;
using System.Linq;
using MOE.Common.Models;
using MOE.Common.Models.Repositories;

namespace MOE.CommonTests.Models
{

    public class InMemoryDirectionTypeRepository : IDirectionTypeRepository
    {
        private InMemoryMOEDatabase _db;

        public InMemoryDirectionTypeRepository()
        {
            _db = new InMemoryMOEDatabase();
            
        }

        public InMemoryDirectionTypeRepository(InMemoryMOEDatabase db)
        {
            _db = db;

        }
        public List<DirectionType> GetAllDirections()
        {
            var dirs = (from r in _db.DirectionTypes
                       select r).ToList();

            return dirs;
        }

        public DirectionType GetDirectionByID(int directionID)
        {
            var dirs = (from r in _db.DirectionTypes
                        where r.DirectionTypeID == directionID
                        select r).FirstOrDefault();

            return dirs;
        }

        public List<System.Web.Mvc.SelectListItem> GetSelectList()
        {
            List<System.Web.Mvc.SelectListItem> list =
                new List<System.Web.Mvc.SelectListItem>();
            foreach (DirectionType d in _db.DirectionTypes.OrderBy(d => d.DisplayOrder))
            {
                list.Add(new System.Web.Mvc.SelectListItem { Value = d.DirectionTypeID.ToString(), Text = d.Description });
            }
            return list;
        }

        public List<DirectionType> GetDirectionsByIDs(List<int> includedDirections)
        {
            return _db.DirectionTypes.Where(d => includedDirections.Contains(d.DirectionTypeID)).ToList();
        }

        public DirectionType GetByDescription(string directionDescription)
        {
            throw new System.NotImplementedException();
        }

        public DirectionType GetByAbbreviation(string abbreviation)
        {
            throw new System.NotImplementedException();
        }

        public DirectionType GetByDescription()
        {
            throw new System.NotImplementedException();
        }
    }
}