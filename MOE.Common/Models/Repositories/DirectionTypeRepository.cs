using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace MOE.Common.Models.Repositories
{
    public class DirectionTypeRepository : IDirectionTypeRepository
    {
        private readonly SPM db = new SPM();

        public List<SelectListItem> GetSelectList()
        {
            var list =
                new List<SelectListItem>();
            foreach (var d in db.DirectionTypes.OrderBy(d => d.DisplayOrder))
                list.Add(new SelectListItem {Value = d.DirectionTypeID.ToString(), Text = d.Description});
            return list;
        }

        public List<DirectionType> GetDirectionsByIDs(List<int> includedDirections)
        {
            return db.DirectionTypes.Where(d => includedDirections.Contains(d.DirectionTypeID)).ToList();
        }

        public DirectionType GetByDescription(string directionDescription)
        {
            return db.DirectionTypes.FirstOrDefault(d => d.Description == directionDescription);
        }

        public DirectionType GetByAbbreviation(string abbreviation)
        {
            return db.DirectionTypes.FirstOrDefault(d => d.Abbreviation == abbreviation);
        }

        public List<DirectionType> GetAllDirections()
        {
            var results = (from r in db.DirectionTypes
                orderby r.DisplayOrder
                select r).ToList();
            return results;
        }

        public DirectionType GetDirectionByID(int DirectionID)
        {
            return db.DirectionTypes.Where(x => x.DirectionTypeID == DirectionID).FirstOrDefault();
        }
    }
}