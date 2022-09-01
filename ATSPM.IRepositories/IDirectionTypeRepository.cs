using ATSPM.Application.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace ATSPM.IRepositories
{
    public interface IDirectionTypeRepository
    {
        List<DirectionType> GetAllDirections();

        DirectionType GetDirectionByID(int directionID);

        List<SelectListItem> GetSelectList();

        List<DirectionType> GetDirectionsByIDs(List<int> includedDirections);
        DirectionType GetByDescription(string directionDescription);
    }
}