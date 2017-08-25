using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Business.ModelObjectHelpers
{

    public class LaneGroupModelHelper
    {
        public Models.LaneGroup LaneGroup { get; set; }
        public String Direction { get; set; }

        public LaneGroupModelHelper(Models.LaneGroup laneGroup)
        {
            //MOE.Common.Models.Repositories.ILaneGroupRepository laneGroups = MOE.Common.Models.Repositories.LaneGroupRepositoryFactory.CreateLaneGroupRepository();

            LaneGroup = laneGroup;

           

        }

        public string GetLaneGroupDirection()
        {

            MOE.Common.Models.Repositories.IApproachRepository Approaches = MOE.Common.Models.Repositories.ApproachRepositoryFactory.Create();
            MOE.Common.Models.Approach approach = Approaches.GetApproachByApproachID(LaneGroup.ApproachID);

            return approach.DirectionType.Description;
        }

        public int GetLaneCount()
        {
            var lanes = (from d in LaneGroup.Detectors
                        select d.LaneNumber).Distinct().ToList();
            return lanes.Count();
        }

        public List<Models.Graph_Detectors> GetDetectorsThatSupportaMetric(int MetricTypeID)
        {
            MOE.Common.Models.Repositories.IGraphDetectorRepository gdr = MOE.Common.Models.Repositories.GraphDetectorRepositoryFactory.Create();

            List<Models.Graph_Detectors> dets = new List<Models.Graph_Detectors>();

            foreach(Models.Graph_Detectors d in LaneGroup.Detectors)
            {
                    if(gdr.CheckReportAvialbility(d.DetectorID, MetricTypeID))
                    {
                        dets.Add(d);
                    }
            }
            

            return dets;
        }
    }
}
