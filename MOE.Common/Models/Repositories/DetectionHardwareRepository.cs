using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Models.Repositories
{
    public class DetectionHardwareRepository : IDetectionHardwareRepository
    {
        Models.SPM db = new SPM();


        public List<Models.DetectionHardware> GetAllDetectionHardwares()
        {
            db.Configuration.LazyLoadingEnabled = false;
            List<Models.DetectionHardware> DetectionHardwares = (from r in db.DetectionHardwares
                                                         select r).ToList();

            return DetectionHardwares;
        }

        public List<Models.DetectionHardware> GetAllDetectionHardwaresNoBasic()
        {
            List<Models.DetectionHardware> DetectionHardwares = (from r in db.DetectionHardwares
                                                         where r.Name != "Basic"
                                                         select r).ToList();

            return DetectionHardwares;
        }



      


        public Models.DetectionHardware GetDetectionHardwareByID(int ID)
        {
            var DetectionHardware = (from r in db.DetectionHardwares
                                 where r.ID == ID
                                 select r);

            return DetectionHardware.FirstOrDefault();
        }

        public List<Models.DetectionHardware> GetDetectionHardwareByIDs(List<int> IDs)
        {
            return (from r in db.DetectionHardwares
                    where IDs.Contains(r.ID)
                    select r).ToList();
        }

        public void Update(MOE.Common.Models.DetectionHardware DetectionHardware)
        {


            MOE.Common.Models.DetectionHardware g = (from r in db.DetectionHardwares
                                                 where r.ID == DetectionHardware.ID
                                                 select r).FirstOrDefault();
            if (g != null)
            {
                db.Entry(g).CurrentValues.SetValues(DetectionHardware);
                db.SaveChanges();
            }
            else
            {
                db.DetectionHardwares.Add(DetectionHardware);
                db.SaveChanges();

            }


        }

        public void Remove(MOE.Common.Models.DetectionHardware DetectionHardware)
        {


            MOE.Common.Models.DetectionHardware g = (from r in db.DetectionHardwares
                                                 where r.ID == DetectionHardware.ID
                                                 select r).FirstOrDefault();
            if (g != null)
            {
                db.DetectionHardwares.Remove(g);
                db.SaveChanges();
            }
        }

        public void Add(MOE.Common.Models.DetectionHardware DetectionHardware)
        {


            MOE.Common.Models.DetectionHardware g = (from r in db.DetectionHardwares
                                                 where r.ID == DetectionHardware.ID
                                                 select r).FirstOrDefault();
            if (g == null)
            {
                db.DetectionHardwares.Add(g);
                db.SaveChanges();
            }

        }

    }
}
