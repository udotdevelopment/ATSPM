using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace MOE.Common.Models.Repositories
{
    public interface IControllerTypeRepository
    {
        List<ControllerType> GetControllerTypes();
        ControllerType GetControllerTypeByDesc(string desc);
    }
}