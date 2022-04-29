using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPM.Models
{
    public class LeftTurnVolumeValueResultViewModel
    {
        public int OpposingLanes { get; set; }
        public bool CrossProductReview { get; set; }
        public bool DecisionBoundariesReview { get; set; }
        public double LeftTurnVolume { get; set; }
        public double OpposingThroughVolume { get; set; }
        public double CrossProductValue { get; set; }
        public double CalculatedVolumeBoundary { get; set; }
        public bool ConsiderForStudy { get;  set; }
        public Dictionary<DateTime, double> DemandList { get; set; }
        public string Direction { get; set; }
        public string OpposingDirection { get; set; }
    }
}