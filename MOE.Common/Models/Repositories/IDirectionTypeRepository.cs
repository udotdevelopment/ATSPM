using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Models.Repositories
{
    public interface IDirectionTypeRepository
    {
        List<Models.DirectionType> GetAllDirections();

        DirectionType GetDirectionByID(int directionID);

        List<System.Web.Mvc.SelectListItem> GetSelectList();

    }
}
