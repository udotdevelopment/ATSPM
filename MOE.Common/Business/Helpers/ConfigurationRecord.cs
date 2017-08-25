using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Business.Helpers
{
    public class ConfigurationRecord
    {
        public string SignalID { get; set; }
        public string DetectorID  { get; set; }
        public string DetectorChannel { get; set; }
        public string ProtectedPhaseNumber { get; set; }
        public string PermissivePhaseNumber { get; set; }
        public string Overlap { get; set; }
        public string Direction { get; set; }
        public string Enabled { get; set; }
        public string DistanceFromStopBar { get; set; }
        public string MPH { get; set; }
        public string DecisionPoint { get; set; }
        public string DetectionTypes { get; set; }
        public string DetectionHardware { get; set; }
        public string MovementDelay { get; set; }
        public string MinSpeedFilter  { get; set; }
        public string LaneNumber { get; set; }
        public string LaneType { get; set; }
        public string MovementType { get; set; }
        public string Comment { get; set; }

        public ConfigurationRecord(MOE.Common.Models.Detector gd)
        {          
            MOE.Common.Models.Repositories.IDetectorRepository gdr = MOE.Common.Models.Repositories.DetectorRepositoryFactory.Create();
            MOE.Common.Models.Repositories.IDetectorCommentRepository dcr = MOE.Common.Models.Repositories.DetectorCommentRepositoryFactory.Create();

            string comment = "";
            Models.DetectorComment c = dcr.GetMostRecentDetectorCommentByDetectorID(gd.ID);

            if(c != null)
            {
                comment = c.CommentText;
            }

            this.Comment = comment;
            this.DecisionPoint = gd.DecisionPoint.ToString();
            this.DetectorChannel = gd.DetChannel.ToString();
            this.DetectorID = gd.DetectorID;
            this.Direction = gd.Approach.DirectionType.Abbreviation;
            this.DistanceFromStopBar = gd.DistanceFromStopBar.ToString();
            this.Enabled = gd.Approach.Signal.Enabled.ToString();
            this.MinSpeedFilter = gd.MinSpeedFilter.ToString();
            this.MovementDelay = gd.MovementDelay.ToString();
            this.MPH = gd.Approach.MPH.ToString();
            this.Overlap = gd.Approach.IsProtectedPhaseOverlap.ToString();
            this.PermissivePhaseNumber = gd.Approach.PermissivePhaseNumber.ToString();
            this.ProtectedPhaseNumber = gd.Approach.ProtectedPhaseNumber.ToString();
            this.DetectionHardware = gd.DetectionHardware.Name;

            if (gd.LaneType != null)
            {
                this.LaneType = gd.LaneType.Description;
            }
            if (gd.LaneNumber != null)
            {
                this.LaneNumber = gd.LaneNumber.ToString();
            }
            if(gd.MovementType !=null)
            {
                this.MovementType = gd.MovementType.Description;
            }
            foreach(MOE.Common.Models.DetectionType dt in gd.DetectionTypes)
            {
                this.DetectionTypes += dt.Description + "<br/>";
            }
            
        }

    }
}
