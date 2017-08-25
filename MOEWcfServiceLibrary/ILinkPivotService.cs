using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.DataVisualization.Charting;

namespace MOEWcfServiceLibrary
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IPCDService" in both code and config file together.
    [ServiceContract]
    public interface ILinkPivotService
    {
        [OperationContract]
        AdjustmentObject[] GetLinkPivot(int routeId, DateTime startDate, DateTime endDate, int cycleTime, string chartLocation,
            string direction, double bias, string biasDirection, bool monday, bool tuesday, bool wednesday,
            bool thursday, bool friday, bool saturday, bool sunday);

        [OperationContract]
        DisplayObject DisplayLinkPivotPCD(string upstreamSignalID, string upstreamDirection,
            string downstreamSignalID, string downstreamDirection, int delta,
            DateTime startDate, DateTime endDate, int maxYAxis);

        [OperationContract]
        int Test();
    }

    [DataContract]
    public class DisplayObject
    {
        private string upstreamBeforePCDPath;
        private string downstreamBeforePCDPath;
        private string upstreamAfterPCDPath;
        private string downstreamAfterPCDPath;
        private double existingAOG;
        private double existingPAOG;
        private double predictedAOG;
        private double predictedPAOG;

        [DataMember]
        public string DownstreamBeforeTitle { get; set; }
        [DataMember]
        public string UpstreamBeforeTitle { get; set; }
        [DataMember]
        public string DownstreamAfterTitle { get; set; }
        [DataMember]
        public string UpstreamAfterTitle { get; set; }
        [DataMember]
        public double PredictedPAOG
        {
            get { return predictedPAOG; }
            set { predictedPAOG = value; }
        }
        [DataMember]
        public double PredictedAOG
        {
            get { return predictedAOG; }
            set { predictedAOG = value; }
        }
        [DataMember]
        public double ExistingPAOG
        {
            get { return existingPAOG; }
            set { existingPAOG = value; }
        }
        [DataMember]
        public double ExistingAOG
        {
            get { return existingAOG; }
            set { existingAOG = value; }
        }
        [DataMember]
        public string UpstreamBeforePCDPath
        {
            get { return upstreamBeforePCDPath; }
            set { upstreamBeforePCDPath = value; }
        }
        [DataMember]
        public string DownstreamBeforePCDPath
        {
            get { return downstreamBeforePCDPath; }
            set { downstreamBeforePCDPath = value; }
        }
        [DataMember]
        public string UpstreamAfterPCDPath
        {
            get { return upstreamAfterPCDPath; }
            set { upstreamAfterPCDPath = value; }
        }
        [DataMember]
        public string DownstreamAfterPCDPath
        {
            get { return downstreamAfterPCDPath; }
            set { downstreamAfterPCDPath = value; }
        }
    }

    [DataContract]
    public class AdjustmentObject
    {
        private string signalId;
        private string location;
        private string downstreamLocation;
        private int delta;
        private int adjustment;
        private double pAOGUpstreamBefore;
        private double pAOGDownstreamBefore;
        private double pAOGUpstreamPredicted;
        private double pAOGDownstreamPredicted;
        private double aOGUpstreamBefore;
        private double aOGDownstreamBefore;
        private double aOGUpstreamPredicted;
        private double aOGDownstreamPredicted;
        private string downSignalId;
        private string downstreamApproachDirection;
        private string upstreamApproachDirection;
        private string resultChartLocation;
        private double _AogTotalBefore;

        private double _DownstreamVolume;
        [DataMember]
        public double DownstreamVolume
        {
            get { return _DownstreamVolume; }
            set { _DownstreamVolume = value; }
        }        

        private double _UpstreamVolume;
        [DataMember]
        public double UpstreamVolume
        {
            get { return _UpstreamVolume; }
            set { _UpstreamVolume = value; }
        }
        
        private int _LinkNumber;
        [DataMember]
        public int LinkNumber
        {
            get { return _LinkNumber; }
            set { _LinkNumber = value; }
        }
        

        [DataMember]
        public double AogTotalBefore
        {
            get { return _AogTotalBefore; }
            set { _AogTotalBefore = value; }
        }

        private double _PAogTotalBefore;
        [DataMember]
        public double PAogTotalBefore
        {
            get { return _PAogTotalBefore; }
            set { _PAogTotalBefore = value; }
        }

        private double _AogTotalPredicted;
        [DataMember]
        public double AogTotalPredicted
        {
            get { return _AogTotalPredicted; }
            set { _AogTotalPredicted = value; }
        }

        private double _PAogTotalPredicted;
        [DataMember]
        public double PAogTotalPredicted
        {
            get { return _PAogTotalPredicted; }
            set { _PAogTotalPredicted = value; }
        }
        
        
        
        
        

        [DataMember]
        public string ResultChartLocation
        {
            get { return resultChartLocation; }
            set { resultChartLocation = value; }
        }

        [DataMember]
        public string DownstreamLocation
        {
            get { return downstreamLocation; }
            set { downstreamLocation = value; }
        }

        [DataMember]
        public string DownSignalId
        {
            get { return downSignalId; }
            set { downSignalId = value; }
        }

        [DataMember]
        public string DownstreamApproachDirection
        {
            get { return downstreamApproachDirection; }
            set { downstreamApproachDirection = value; }
        }

        [DataMember]
        public string UpstreamApproachDirection
        {
            get { return upstreamApproachDirection; }
            set { upstreamApproachDirection = value; }
        }

        [DataMember]
        public double AOGDownstreamPredicted
        {
            get { return aOGDownstreamPredicted; }
            set { aOGDownstreamPredicted = value; }
        }

        [DataMember]
        public double AOGUpstreamPredicted
        {
            get { return aOGUpstreamPredicted; }
            set { aOGUpstreamPredicted = value; }
        }

        [DataMember]
        public double AOGDownstreamBefore
        {
            get { return aOGDownstreamBefore; }
            set { aOGDownstreamBefore = value; }
        }

        [DataMember]
        public double AOGUpstreamBefore
        {
            get { return aOGUpstreamBefore; }
            set { aOGUpstreamBefore = value; }
        }

        [DataMember]
        public double PAOGDownstreamPredicted
        {
            get { return pAOGDownstreamPredicted; }
            set { pAOGDownstreamPredicted = value; }
        }

        [DataMember]
        public double PAOGUpstreamPredicted
        {
            get { return pAOGUpstreamPredicted; }
            set { pAOGUpstreamPredicted = value; }
        }

        [DataMember]
        public double PAOGDownstreamBefore
        {
            get { return pAOGDownstreamBefore; }
            set { pAOGDownstreamBefore = value; }
        }

        [DataMember]
        public double PAOGUpstreamBefore
        {
            get { return pAOGUpstreamBefore; }
            set { pAOGUpstreamBefore = value; }
        }

        [DataMember]
        public int Adjustment
        {
            get { return adjustment; }
            set { adjustment = value; }
        }

        [DataMember]
        public int Delta
        {
            get { return delta; }
            set { delta = value; }
        }

        [DataMember]
        public string SignalId
        {
            get { return signalId; }
            set { signalId = value; }
        }

        [DataMember]
        public string Location
        {
            get { return location; }
            set { location = value; }
        }



    }
}
