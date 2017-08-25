using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Models.Repositories
{
   public class MovementTypeRepositoryFactory
   {
       private static IMovementTypeRepository movementTypeRepository;

       public static IMovementTypeRepository Create()
       {
           if (movementTypeRepository != null)
           {
               return movementTypeRepository;
           }
           return new MovementTypeRepository();
       }

       public static void SetMovementTypeRepository(IMovementTypeRepository newRepository)
       {
           movementTypeRepository = newRepository;
       }
   }
}
