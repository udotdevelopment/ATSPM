using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.WebPages.Html;
using System.Web.Mvc;

namespace MOE.Common.Models.Repositories
{
    public class DirectionTypeRepository : IDirectionTypeRepository
    {
        Models.SPM db = new SPM();

        public List<System.Web.Mvc.SelectListItem> GetSelectList()
        {
            List<System.Web.Mvc.SelectListItem> list =
                new List<System.Web.Mvc.SelectListItem>();
            foreach(DirectionType d in db.DirectionTypes.OrderBy(d => d.DisplayOrder))
            {
                list.Add(new System.Web.Mvc.SelectListItem { Value = d.DirectionTypeID.ToString(), Text = d.Description });
            }
            return list;
        }
        public List<Models.DirectionType> GetAllDirections()
        {
            List<Models.DirectionType> results = (from r in db.DirectionTypes
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
