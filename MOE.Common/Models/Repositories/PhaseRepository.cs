using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Models.Repositories
{
    class PhaseRepository : IPhaseRepository
    {
        Models.SPM db = new SPM();
       

        public List<Models.Phase> GetAllPhases()
        {
            db.Configuration.LazyLoadingEnabled = false;
            List<Models.Phase> phases = (from r in db.Phases
                                           select r).ToList();

            return phases;
        }

        public Models.Phase GetPhaseByPhaseID(int phaseID)
        {
            var phase = (from r in db.Phases
                         where r.PhaseID == phaseID
                          select r);

            return phase.FirstOrDefault();
        }

        public void Update(MOE.Common.Models.Phase phase)
        {


            MOE.Common.Models.Phase g = (from r in db.Phases
                                         where r.PhaseID == phase.PhaseID
                                          select r).FirstOrDefault();
            if (g != null)
            {
                db.Entry(g).CurrentValues.SetValues(phase);
                db.SaveChanges();
            }
            else
            {
                db.Phases.Add(phase);
                db.SaveChanges();

            }


        }

        public void Remove(MOE.Common.Models.Phase phase)
        {


            MOE.Common.Models.Phase g = (from r in db.Phases
                                          where r.PhaseID == phase.PhaseID
                                          select r).FirstOrDefault();
            if (g != null)
            {
                db.Phases.Remove(g);
                db.SaveChanges();
            }
        }

        public void Add(MOE.Common.Models.Phase phase)
        {


            MOE.Common.Models.Phase g = (from r in db.Phases
                                          where r.PhaseID == phase.PhaseID
                                          select r).FirstOrDefault();
            if (g == null)
            {
                db.Phases.Add(g);
                db.SaveChanges();
            }

        }

    }
}
