using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Business.ModelObjectHelpers
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
        public string PCDEnabled { get; set; }
        public string DistanceFromStopBar { get; set; }
        public string MPH { get; set; }
        public string DecisionPoint { get; set; }
        public string SpeedEnabled { get; set; }
        public string MovementDelay { get; set; }
        public string MinSpeedFilter  { get; set; }
        public string TMCEnabled { get; set; }
        public string YRAEnabled { get; set; }
        public string SplitFailEnabled { get; set; }
        public string LaneType { get; set; }
        public string Comment { get; set; }





        public ConfigurationRecord(MOE.Common.Models.Graph_Detectors gd)
        {
           
            MOE.Common.Models.Repositories.IGraphDetectorRepository gdr = MOE.Common.Models.Repositories.GraphDetectorRepositoryFactory.Create();
            MOE.Common.Models.Repositories.IDetectorCommentRepository dcr = MOE.Common.Models.Repositories.DetectorCommentRepositoryFactory.Create();

            string comment = "";
            Models.DetectorComment c = dcr.GetMostRecentDetectorCommentByDetectorID(gd.DetectorID);

            if(c != null)
            {
                comment = c.CommentText;
            }



            this.Comment = comment;
            this.DecisionPoint = gd.LaneGroup.Approach.Decision_Point.ToString();
            this.DetectorChannel = gd.Det_Channel.ToString();
            this.DetectorID = gd.DetectorID;
            this.Direction = gd.LaneGroup.Approach.DirectionType.Description;
            this.DistanceFromStopBar = gd.DistanceFromStopBar.ToString();
            this.Enabled = gd.LaneGroup.Approach.Signal.Enabled.ToString();
            this.LaneType = gd.LaneGroup.MovementType.Abbreviation + gd.LaneNumber.ToString();
            this.MinSpeedFilter = gd.Min_Speed_Filter.ToString();
            this.MovementDelay = gd.LaneGroup.Approach.Movement_Delay.ToString();
            this.MPH = gd.LaneGroup.Approach.MPH.ToString();
            this.Overlap = gd.LaneGroup.IsProtectedPhaseOverlap.ToString();
            this.PCDEnabled = gdr.CheckReportAvialbility(gd.DetectorID, 6).ToString();
            this.PermissivePhaseNumber = gd.LaneGroup.PermissivePhaseNumber.ToString();
            this.ProtectedPhaseNumber = gd.LaneGroup.ProtectedPhaseNumber.ToString();
            this.SpeedEnabled = gdr.CheckReportAvialbility(gd.DetectorID, 10).ToString();
            this.SplitFailEnabled = gdr.CheckReportAvialbility(gd.DetectorID, 12).ToString();
            this.TMCEnabled = gdr.CheckReportAvialbility(gd.DetectorID, 5).ToString();
            this.YRAEnabled = gdr.CheckReportAvialbility(gd.DetectorID, 11).ToString();

            
        }

    }
}
