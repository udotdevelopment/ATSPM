using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Models.Repositories
{
    public class DirectionTypeRepositoryFactory
    {
        private static IDirectionTypeRepository DirectionTypeRepository;

        public static IDirectionTypeRepository Create()
        {
            if (DirectionTypeRepository != null)
            {
                return DirectionTypeRepository;
            }
            return new DirectionTypeRepository();
        }

        public static void SetDirectionsRepository(IDirectionTypeRepository newRepository)
        {
            DirectionTypeRepository = newRepository;
        }
    }
}
