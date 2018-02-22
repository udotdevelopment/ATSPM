using MOE.Common.Models.Repositories;

namespace MOE.Common.Business.Helpers
{
    public class ConfigurationRecord
    {
        public ConfigurationRecord(Models.Detector gd)
        {
            var gdr = DetectorRepositoryFactory.Create();
            var dcr = DetectorCommentRepositoryFactory.Create();

            var comment = "";
            var c = dcr.GetMostRecentDetectorCommentByDetectorID(gd.ID);

            if (c != null)
                comment = c.CommentText;

            Comment = comment;
            DecisionPoint = gd.DecisionPoint.ToString();
            DetectorChannel = gd.DetChannel.ToString();
            DetectorID = gd.DetectorID;
            Direction = gd.Approach.DirectionType.Abbreviation;
            DistanceFromStopBar = gd.DistanceFromStopBar.ToString();
            Enabled = gd.Approach.Signal.Enabled.ToString();
            MinSpeedFilter = gd.MinSpeedFilter.ToString();
            MovementDelay = gd.MovementDelay.ToString();
            MPH = gd.Approach.MPH.ToString();
            Overlap = gd.Approach.IsProtectedPhaseOverlap.ToString();
            PermissivePhaseNumber = gd.Approach.PermissivePhaseNumber.ToString();
            ProtectedPhaseNumber = gd.Approach.ProtectedPhaseNumber.ToString();
            DetectionHardware = gd.DetectionHardware.Name;
            LatencyCorrection = gd.LatencyCorrection.ToString();

            if (gd.LaneType != null)
                LaneType = gd.LaneType.Description;
            if (gd.LaneNumber != null)
                LaneNumber = gd.LaneNumber.ToString();
            if (gd.MovementType != null)
                MovementType = gd.MovementType.Description;
            foreach (var dt in gd.DetectionTypes)
                DetectionTypes += dt.Description + "<br/>";
        }

        public string SignalID { get; set; }
        public string DetectorID { get; set; }
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
        public string MinSpeedFilter { get; set; }
        public string LaneNumber { get; set; }
        public string LaneType { get; set; }
        public string MovementType { get; set; }
        public string Comment { get; set; }
        public string LatencyCorrection { get; set; }
    }
}