namespace MOE.Common.Business.Helpers
{
    public class ApproachVolumeTable
    {
        public string TotalVolume { get; set; }
        public string PeakHour { get; set; }
        public string PeakHourVolume { get; set; }
        public string PeakHourKFactor { get; set; }

        public string DirectionOne { get; set; }
        public string DirectionTwo { get; set; }

        public string D1TotalVolume { get; set; }
        public string D1PeakHour { get; set; }
        public string D1PeakHourVolume { get; set; }
        public string D1PHF { get; set; }
        public string D1PeakHourKFactor { get; set; }
        public string D1PeakHourDFactor { get; set; }

        public string D2TotalVolume { get; set; }
        public string D2PeakHour { get; set; }
        public string D2PeakHourVolume { get; set; }
        public string D2PHF { get; set; }
        public string D2PeakHourKFactor { get; set; }
        public string D2PeakHourDFactor { get; set; }


        //public ApproachVolumeTable(MOE.Common.Models.Detector gd)
        //{

        //    MOE.Common.Models.Repositories.IDetectorRepository gdr = MOE.Common.Models.Repositories.DetectorRepositoryFactory.Create();
        //    MOE.Common.Models.Repositories.IDetectorCommentRepository dcr = MOE.Common.Models.Repositories.DetectorCommentRepositoryFactory.Create();

        //    string comment = "";
        //    Models.DetectorComment c = dcr.GetMostRecentDetectorCommentByDetectorID(gd.ID);

        //    if(c != null)
        //    {
        //        comment = c.CommentText;
        //    }


        //}
    }
}