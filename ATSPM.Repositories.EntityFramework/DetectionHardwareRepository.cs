using ATSPM.IRepositories;
using ATSPM.Application.Models;
using System.Collections.Generic;
using System.Linq;

namespace ATSPM.Infrastructure.Repositories.EntityFramework
{
    public class DetectionHardwareRepository : IDetectionHardwareRepository
    {
        private readonly MOEContext db;

        public DetectionHardwareRepository(MOEContext db)
        {
            this.db = db;
        }

        public List<DetectionHardware> GetAllDetectionHardwares()
        {
            //db.Configuration.LazyLoadingEnabled = false;
            var DetectionHardwares = (from r in db.DetectionHardwares
                                      select r).ToList();

            return DetectionHardwares;
        }

        public List<DetectionHardware> GetAllDetectionHardwaresNoBasic()
        {
            var DetectionHardwares = (from r in db.DetectionHardwares
                                      where r.Name != "Basic"
                                      select r).ToList();

            return DetectionHardwares;
        }


        public DetectionHardware GetDetectionHardwareByID(int ID)
        {
            var DetectionHardware = from r in db.DetectionHardwares
                                    where r.Id == ID
                                    select r;

            return DetectionHardware.FirstOrDefault();
        }

        public void Update(DetectionHardware DetectionHardware)
        {
            var g = (from r in db.DetectionHardwares
                     where r.Id == DetectionHardware.Id
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

        public void Remove(DetectionHardware DetectionHardware)
        {
            var g = (from r in db.DetectionHardwares
                     where r.Id == DetectionHardware.Id
                     select r).FirstOrDefault();
            if (g != null)
            {
                db.DetectionHardwares.Remove(g);
                db.SaveChanges();
            }
        }

        public void Add(DetectionHardware DetectionHardware)
        {
            var g = (from r in db.DetectionHardwares
                     where r.Id == DetectionHardware.Id
                     select r).FirstOrDefault();
            if (g == null)
            {
                db.DetectionHardwares.Add(g);
                db.SaveChanges();
            }
        }

        public List<DetectionHardware> GetDetectionHardwareByIDs(List<int> IDs)
        {
            return (from r in db.DetectionHardwares
                    where IDs.Contains(r.Id)
                    select r).ToList();
        }
    }
}