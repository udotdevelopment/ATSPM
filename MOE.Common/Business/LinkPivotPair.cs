using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.DataVisualization.Charting;

namespace MOE.Common.Business
{
    public class LinkPivotPair
    {
        private string signalId;
        public string SignalId
        {
            get { return signalId; }
        }
        string signalDirection;

        private string downSignalId;
        public string DownSignalId
        {
            get { return downSignalId; }
        }
        
        string downSignalDirection;
        public string DownSignalDirection
        {
            get { return downSignalDirection; }
        }

        private double secondsAdded = 0;
        public double SecondsAdded
        {
            get { return secondsAdded; }
        }

        private double maxArrivalOnGreen = 0;
        public double MaxArrivalOnGreen
        {
            get { return maxArrivalOnGreen; }
        }

        private double maxPercentAOG = 0;
        public double MaxPercentAOG
        {
            get { return maxPercentAOG; }
        }

        private int upstreamApproach;
        
        private List<MOE.Common.Business.SignalPhase> upstreamPCD = new List<SignalPhase>();
        public List<MOE.Common.Business.SignalPhase> UpstreamPCD
        {
            get { return upstreamPCD; }
        }
       
        
        private List<MOE.Common.Business.SignalPhase> downstreamPCD = new List<SignalPhase>();
        public List<MOE.Common.Business.SignalPhase> DownstreamPCD
        {
            get { return downstreamPCD; }
        }

        private double pAOGUpstreamBefore=0;
        public double PAOGUpstreamBefore
        {
            get { return pAOGUpstreamBefore;}
        }

        private double pAOGDownstreamBefore = 0;
        public double PAOGDownstreamBefore
        {
            get { return pAOGDownstreamBefore; }
        }

        private double pAOGDownstreamPredicted = 0;
        public double PAOGDownstreamPredicted
        {
            get { return pAOGDownstreamPredicted; }
        }

        private double pAOGUpstreamPredicted = 0;
        public double PAOGUpstreamPredicted
        {
            get { return pAOGUpstreamPredicted; }
        }


        private double aOGUpstreamBefore = 0;
        public double AOGUpstreamBefore
        {
            get { return aOGUpstreamBefore; }
        }

        private double aOGDownstreamBefore = 0;
        public double AOGDownstreamBefore
        {
            get { return aOGDownstreamBefore; }
        }

        private double aOGDownstreamPredicted = 0;
        public double AOGDownstreamPredicted
        {
            get { return aOGDownstreamPredicted; }
        }

        private double aOGUpstreamPredicted = 0;
        public double AOGUpstreamPredicted
        {
            get { return aOGUpstreamPredicted; }
        }

        private double totalVolumeUpstream = 0;
        public double TotalVolumeUpstream
        {
            get { return totalVolumeUpstream; }
        }

        private double totalVolumeDownstream = 0;
        public double TotalVolumeDownstream
        {
            get { return totalVolumeDownstream; }
        }

        private string location;
        public string Location
        {
            get { return location; }
        }

        private string downstreamLocation;
        public string DownstreamLocation
        {
            get { return downstreamLocation; }
        }

        private string downstreamApproachDirection;
        public string DownstreamApproachDirection
        {
            get { return downstreamApproachDirection; }
        }

        private string upstreamApproachDirection;
        public string UpstreamApproachDirection
        {
            get { return upstreamApproachDirection; }
        }

        private string resultChartLocation;
        public string ResultChartLocation
        {
            get { return resultChartLocation; }
        }

        private Dictionary<int, double> resultsGraph = new Dictionary<int, double>();
        public Dictionary<int, double> ResultsGraph
        {
            get { return resultsGraph; }
        }

        private Dictionary<int, double> upstreamResultsGraph = new Dictionary<int, double>();
        public Dictionary<int, double> UpstreamResultsGraph
        {
            get { return upstreamResultsGraph; }
        }

        private Dictionary<int, double> downstreamResultsGraph = new Dictionary<int, double>();
        public Dictionary<int, double> DownstreamResultsGraph
        {
            get { return downstreamResultsGraph; }
        }

        private List<LinkPivotPCDDisplay> display = new List<LinkPivotPCDDisplay>();
        public List<LinkPivotPCDDisplay> Display
        {
            get { return display; }
        }

        private double _AOGTotalBefore;

        public double AOGTotalBefore
        {
            get { return _AOGTotalBefore; }
            set { _AOGTotalBefore = value; }
        }

        private double _PAOGTotalBefore;

        public double PAOGTotalBefore
        {
            get { return _PAOGTotalBefore; }
            set { _PAOGTotalBefore = value; }
        }

        private double _AOGTotalPredicted;

        public double AOGTotalPredicted
        {
            get { return _AOGTotalPredicted; }
            set { _AOGTotalPredicted = value; }
        }

        private double _PAOGTotalPredicted;

        public double PAOGTotalPredicted
        {
            get { return _PAOGTotalPredicted; }
            set { _PAOGTotalPredicted = value; }
        }

        private int _LinkNumber;

        public int LinkNumber
        {
            get { return _LinkNumber; }
            set { _LinkNumber = value; }
        }
        
        
        
        
        

        /// <summary>
        /// Represents a pair of approaches to be compared for link pivot anaylisis
        /// </summary>
        /// <param name="signalId"></param>
        /// <param name="signalDirection"></param>
        /// <param name="signalLocation"></param>
        /// <param name="downSignalId"></param>
        /// <param name="downSignalDirection"></param>
        /// <param name="downSignalLocation"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="cycleTime"></param>
        /// <param name="chartLocation"></param>
        /// <param name="bias"></param>
        /// <param name="biasDirection"></param>
        /// <param name="dates"></param>
        public LinkPivotPair(string signalId, string signalDirection, string signalLocation, string downSignalId, 
            string downSignalDirection, string downSignalLocation,
            DateTime startDate, DateTime endDate, int cycleTime, string chartLocation
            , double bias, string biasDirection, List<DateTime> dates, int linkNumber)
        {            
            this.signalId = signalId;
            this.signalDirection = signalDirection;
            this.downSignalId = downSignalId;
            this.downSignalDirection = downSignalDirection;
            this.downstreamApproachDirection = downSignalDirection;
            this.location = signalLocation;
            this.downstreamLocation = downSignalLocation;
            this._LinkNumber = linkNumber;
             
            //Determine the direction of the approach to be analyzed. This will determine
            //which approches are compared as downstream and upstream.
                        
            if(signalDirection == "Northbound")
            {
                upstreamApproachDirection = "Southbound";
            }
            else if (signalDirection == "Southbound")
            {
                upstreamApproachDirection = "Northbound";
            }
            else if (signalDirection == "Eastbound")
            {
                upstreamApproachDirection = "Westbound";
            }
            else if (signalDirection == "Westbound")
            {
                upstreamApproachDirection = "Eastbound";
            }
            
            
            //MOE.Common.Data.LinkPivotTableAdapters.Graph_DetectorsTableAdapter gdAdapter =
            //   new Data.LinkPivotTableAdapters.Graph_DetectorsTableAdapter(); 

            MOE.Common.Models.SPM db = new Models.SPM();

            //find the upstream approach
            if (!String.IsNullOrEmpty(upstreamApproachDirection))
            {
                MOE.Common.Models.Repositories.ISignalsRepository repository =
                MOE.Common.Models.Repositories.SignalsRepositoryFactory.Create();
                var signal = repository.GetSignalBySignalID(signalId);
                List<MOE.Common.Models.Detector> dets = 
                    signal.GetDetectorsForSignalThatSupportAMetricByApproachDirection(6, upstreamApproachDirection);
                foreach (MOE.Common.Models.Detector row in dets)
                {
                        //Get the upstream pcd
                        foreach (DateTime dt in dates)
                        {
                            DateTime tempStartDate = dt;
                            DateTime tempEndDate = new DateTime(dt.Year, dt.Month, dt.Day, endDate.Hour, 
                                endDate.Minute, endDate.Second);
                            
                            SignalPhase usp = new MOE.Common.Business.SignalPhase(
                                            tempStartDate, tempEndDate, row.Approach, false, 15,13);
                            upstreamPCD.Add(usp);
                            aOGUpstreamBefore += usp.TotalArrivalOnGreen;
                            totalVolumeUpstream += usp.TotalVolume;
                            
                        }
                        pAOGUpstreamBefore = Math.Round(aOGUpstreamBefore / totalVolumeUpstream, 2) * 100;
                        //aOGUpstreamBefore = upstreamPCD.TotalArrivalOnGreen;
                        //upstreamBeforePCDPath = CreateChart(upstreamPCD, startDate, endDate, signalLocation, 
                         //   "before", chartLocation);
                    
                }
            }

            //find the downstream approach
            if (!String.IsNullOrEmpty(downSignalDirection))
            {
                MOE.Common.Models.Repositories.ISignalsRepository repository =
                MOE.Common.Models.Repositories.SignalsRepositoryFactory.Create();
                var signal = repository.GetSignalBySignalID(downSignalId);
                List<MOE.Common.Models.Detector> dets = 
                    signal.GetDetectorsForSignalThatSupportAMetricByApproachDirection(6, downSignalDirection);
                foreach (MOE.Common.Models.Detector row in dets)
                {
                    //Get the downstream pcd
                    foreach (DateTime dt in dates)
                    {
                        DateTime tempStartDate = dt;
                        DateTime tempEndDate = new DateTime(dt.Year, dt.Month, dt.Day, endDate.Hour, endDate.Minute, endDate.Second);

                        SignalPhase dsp = new MOE.Common.Business.SignalPhase(
                                        tempStartDate, tempEndDate, row.Approach, false, 15,13);
                        downstreamPCD.Add(dsp);
                        aOGDownstreamBefore += dsp.TotalArrivalOnGreen;
                        totalVolumeDownstream += dsp.TotalVolume;

                    }

                    pAOGDownstreamBefore = Math.Round(aOGDownstreamBefore / totalVolumeDownstream, 2) * 100;
                    //aOGDownstreamBefore = downstreamPCD.TotalArrivalOnGreen;
                    //downstreamBeforePCDPath = CreateChart(downstreamPCD, startDate, endDate, downSignalLocation, 
                    //    "before", chartLocation);
                }

            }

            //Check to see if both directions have detection if so analyze both
            if (upstreamPCD.Count > 0 && downstreamPCD.Count > 0)
            {
                //Data.LinkPivotTableAdapters.Graph_DetectorsTableAdapter gda = 
                //    new Data.LinkPivotTableAdapters.Graph_DetectorsTableAdapter();
                //int? recordedCycleTime = gda.GetCycleTime(startDate, Convert.ToInt32(signalId));

                //if a bias was provided bias the results in the direction specified
                if (bias != 0)
                {
                    double upstreamBias = 1;
                    double downstreamBias = 1;
                    if (biasDirection == "Downstream")
                    {
                        downstreamBias = 1 + (bias/100);
                    }
                    else
                    {
                        upstreamBias = 1 + (bias / 100);
                    }
                    //set the original values to compare against
                    _AOGTotalBefore = (aOGDownstreamBefore * downstreamBias) + 
                        (aOGUpstreamBefore * upstreamBias);
                    _PAOGTotalBefore = Math.Round(_AOGTotalBefore/((totalVolumeDownstream*downstreamBias)+(totalVolumeUpstream*upstreamBias)),2)*100;
                    double maxBiasArrivalOnGreen = _AOGTotalBefore;
                    maxArrivalOnGreen = aOGDownstreamBefore + aOGUpstreamBefore;


                    //add the total to the results grid
                    resultsGraph.Add(0, maxArrivalOnGreen);
                    upstreamResultsGraph.Add(0, aOGUpstreamBefore * upstreamBias);
                    downstreamResultsGraph.Add(0, aOGDownstreamBefore * downstreamBias);
                    aOGUpstreamPredicted = aOGUpstreamBefore;
                    aOGDownstreamPredicted = aOGDownstreamBefore;
                    pAOGDownstreamPredicted = Math.Round(aOGDownstreamBefore / totalVolumeDownstream, 2)*100 ;
                    pAOGUpstreamPredicted = Math.Round(aOGUpstreamBefore / totalVolumeUpstream, 2)*100;
                    secondsAdded = 0;

                    for (int i = 1; i <= cycleTime; i++)
                    {
                        double totalBiasArrivalOnGreen = 0;
                        double totalArrivalOnGreen = 0;
                        double totalUpstreamAog = 0;
                        double totalDownstreamAog = 0;

                        for (int index = 0; index < dates.Count; index++)
                        {
                            upstreamPCD[index].LinkPivotAddSeconds(-1);
                            downstreamPCD[index].LinkPivotAddSeconds(1);
                            totalBiasArrivalOnGreen += (upstreamPCD[index].TotalArrivalOnGreen * upstreamBias) +
                                (downstreamPCD[index].TotalArrivalOnGreen * downstreamBias);
                            totalArrivalOnGreen =+ (upstreamPCD[index].TotalArrivalOnGreen) +
                                (downstreamPCD[index].TotalArrivalOnGreen);
                            totalUpstreamAog += upstreamPCD[index].TotalArrivalOnGreen;
                            totalDownstreamAog += downstreamPCD[index].TotalArrivalOnGreen;
                            
                        }
                        //Add the total aog to the dictionary
                        resultsGraph.Add(i, totalBiasArrivalOnGreen);
                        upstreamResultsGraph.Add(i, totalUpstreamAog);
                        downstreamResultsGraph.Add(i, totalDownstreamAog);

                        if (totalBiasArrivalOnGreen > maxBiasArrivalOnGreen)
                        {
                            maxBiasArrivalOnGreen = totalBiasArrivalOnGreen;
                            maxArrivalOnGreen = totalArrivalOnGreen;
                            aOGUpstreamPredicted = totalUpstreamAog;
                            aOGDownstreamPredicted = totalDownstreamAog;
                            pAOGDownstreamPredicted = Math.Round(totalDownstreamAog / totalVolumeDownstream, 2)*100;
                            pAOGUpstreamPredicted = Math.Round(totalUpstreamAog / totalVolumeUpstream, 2)*100;
                            maxPercentAOG = 
                            secondsAdded = i;
                        }
                    }
                    //Get the link totals
                    _AOGTotalPredicted = maxArrivalOnGreen;
                    _PAOGTotalPredicted = Math.Round(_AOGTotalPredicted / (totalVolumeUpstream + totalVolumeDownstream), 2)*100;
                }
                //If no bias is provided
                else
                {
                    //set the original values to compare against
                    _AOGTotalBefore = aOGDownstreamBefore + aOGUpstreamBefore;
                    maxArrivalOnGreen = _AOGTotalBefore;
                    _PAOGTotalBefore = Math.Round(_AOGTotalBefore / (totalVolumeDownstream + totalVolumeUpstream),2)*100;
                    //add the total to the results grid
                    resultsGraph.Add(0, maxArrivalOnGreen);
                    upstreamResultsGraph.Add(0, aOGUpstreamBefore);
                    downstreamResultsGraph.Add(0, aOGDownstreamBefore);

                    aOGUpstreamPredicted = aOGUpstreamBefore;
                    aOGDownstreamPredicted = aOGDownstreamBefore;
                    pAOGDownstreamPredicted = Math.Round(aOGDownstreamBefore / totalVolumeDownstream, 2)*100;
                    pAOGUpstreamPredicted = Math.Round(aOGUpstreamBefore / totalVolumeUpstream, 2)*100;
                    secondsAdded = 0;

                    for (int i = 1; i <= cycleTime; i++)
                    {
                        
                        double totalArrivalOnGreen = 0;
                        double totalUpstreamAog = 0;
                        double totalDownstreamAog = 0;                        

                        for (int index = 0; index < dates.Count; index++)
                        {
                            upstreamPCD[index].LinkPivotAddSeconds(-1);
                            downstreamPCD[index].LinkPivotAddSeconds(1);                            
                            totalArrivalOnGreen += (upstreamPCD[index].TotalArrivalOnGreen) +
                                (downstreamPCD[index].TotalArrivalOnGreen);
                            totalUpstreamAog += upstreamPCD[index].TotalArrivalOnGreen;
                            totalDownstreamAog += downstreamPCD[index].TotalArrivalOnGreen;
                        }
                        //Add the total aog to the dictionary
                        resultsGraph.Add(i, totalArrivalOnGreen);
                        upstreamResultsGraph.Add(i, totalUpstreamAog);
                        downstreamResultsGraph.Add(i, totalDownstreamAog);

                        if (totalArrivalOnGreen > maxArrivalOnGreen)
                        {                            
                            maxArrivalOnGreen = totalArrivalOnGreen;
                            aOGUpstreamPredicted = totalUpstreamAog;
                            aOGDownstreamPredicted = totalDownstreamAog;
                            pAOGDownstreamPredicted = Math.Round(totalDownstreamAog / totalVolumeDownstream, 2)*100;
                            pAOGUpstreamPredicted = Math.Round(totalUpstreamAog / totalVolumeUpstream, 2)*100;
                            secondsAdded = i;
                        }
                    }
                    //Get the link totals
                    _AOGTotalPredicted = maxArrivalOnGreen;
                    _PAOGTotalPredicted = Math.Round(_AOGTotalPredicted / (totalVolumeUpstream + totalVolumeDownstream), 2)*100;
                }

                //remove the seconds from the pcd to produce the optimized pcd
                //int secondsToRemove = Convert.ToInt32(cycleTime - secondsAdded);
                //upstreamPCD.LinkPivotAddSeconds(secondsToRemove);
                //downstreamPCD.LinkPivotAddSeconds(secondsToRemove*-1);

                //pAOGUpstreamPredicted = upstreamPCD.PercentArrivalOnGreen;
                //aOGUpstreamPredicted = upstreamPCD.TotalArrivalOnGreen;
                //pAOGDownstreamPredicted = downstreamPCD.PercentArrivalOnGreen;
                //aOGDownstreamPredicted = downstreamPCD.TotalArrivalOnGreen;

                //upstreamAfterPCDPath = CreateChart(upstreamPCD, startDate, endDate, signalLocation,
                //            "after", chartLocation);
                //downstreamAfterPCDPath = CreateChart(downstreamPCD, startDate, endDate, downSignalLocation,
                //            "after", chartLocation);
            }
            //If only upstream has detection do analysis for upstream only
            else if (downstreamPCD.Count == 0 && upstreamPCD.Count > 0)
            {
                //if a bias was provided by the user apply it to the upstream results
                if (bias != 0)
                {
                    double upstreamBias = 1;
                    double downstreamBias = 1;
                    if (biasDirection == "Downstream")
                    {
                        downstreamBias = 1 + (bias / 100);
                    }
                    else
                    {
                        upstreamBias = 1 + (bias / 100);
                    }
                    //set the original values to compare against
                    double maxBiasArrivalOnGreen = (aOGDownstreamBefore * downstreamBias) +
                        (aOGUpstreamBefore * upstreamBias);
                    maxArrivalOnGreen = aOGUpstreamBefore;
                    //Add the total to the results grid
                    resultsGraph.Add(0, maxArrivalOnGreen);
                    upstreamResultsGraph.Add(0, aOGUpstreamBefore * upstreamBias);
                    downstreamResultsGraph.Add(0, aOGDownstreamBefore * downstreamBias);
                    aOGUpstreamPredicted = aOGUpstreamBefore;
                    pAOGUpstreamPredicted = Math.Round(aOGUpstreamBefore / totalVolumeUpstream, 2)*100;
                    secondsAdded = 0;
                    
                    for (int i = 1; i <= cycleTime; i++)
                    {
                        double totalBiasArrivalOnGreen = 0;
                        double totalArrivalOnGreen = 0;
                        double totalUpstreamAog = 0;
                       

                        for (int index = 0; index < dates.Count; index++)
                        {
                            upstreamPCD[index].LinkPivotAddSeconds(-1);
                            
                            totalBiasArrivalOnGreen += (upstreamPCD[index].TotalArrivalOnGreen * upstreamBias);
                            totalArrivalOnGreen = +(upstreamPCD[index].TotalArrivalOnGreen);                            
                            totalUpstreamAog += upstreamPCD[index].TotalArrivalOnGreen;                           
                        }
                        //Add the total aog to the dictionary
                        resultsGraph.Add(i, totalBiasArrivalOnGreen);
                        upstreamResultsGraph.Add(i, totalUpstreamAog);

                        if (totalBiasArrivalOnGreen > maxBiasArrivalOnGreen)
                        {
                            maxBiasArrivalOnGreen = totalBiasArrivalOnGreen;
                            maxArrivalOnGreen = totalArrivalOnGreen;
                            aOGUpstreamPredicted = totalUpstreamAog;                           
                            pAOGUpstreamPredicted = Math.Round(totalUpstreamAog / totalVolumeUpstream, 2)*100;
                            secondsAdded = i;
                        }
                    }
                    //Get the link totals
                    _AOGTotalPredicted = maxArrivalOnGreen;
                    _PAOGTotalPredicted = pAOGUpstreamPredicted;
                }
                //No bias provided
                else
                {
                    //set the original values to compare against
                    _AOGTotalBefore = aOGDownstreamBefore + aOGUpstreamBefore;
                    maxArrivalOnGreen = aOGUpstreamBefore;
                    _PAOGTotalBefore = Math.Round(_AOGTotalBefore / (totalVolumeDownstream + totalVolumeUpstream), 2) * 100;
                    
                    
                    //Add the total aog to the dictionary
                    resultsGraph.Add(0, maxArrivalOnGreen);
                    upstreamResultsGraph.Add(0, aOGUpstreamBefore);
                    downstreamResultsGraph.Add(0, aOGDownstreamBefore);
                    aOGUpstreamPredicted = aOGUpstreamBefore;
                    pAOGUpstreamPredicted = Math.Round(aOGUpstreamBefore / totalVolumeUpstream, 2)*100;
                    secondsAdded = 0;
                    for (int i = 1; i <= cycleTime; i++)
                    {

                        double totalArrivalOnGreen = 0;
                        double totalUpstreamAog = 0;                       

                        for (int index = 0; index < dates.Count; index++)
                        {
                            upstreamPCD[index].LinkPivotAddSeconds(-1);                            
                            totalArrivalOnGreen = +(upstreamPCD[index].TotalArrivalOnGreen);
                            totalUpstreamAog += upstreamPCD[index].TotalArrivalOnGreen;
                            
                        }
                        //Add the total aog to the dictionary
                        resultsGraph.Add(i, totalArrivalOnGreen);
                        upstreamResultsGraph.Add(i, totalUpstreamAog);

                        if (totalArrivalOnGreen > maxArrivalOnGreen)
                        {
                            maxArrivalOnGreen = totalArrivalOnGreen;
                            aOGUpstreamPredicted = totalUpstreamAog;
                            pAOGUpstreamPredicted = Math.Round(totalUpstreamAog / totalVolumeUpstream, 2)*100;
                            secondsAdded = i;
                        }
                        //Get the link totals
                        _AOGTotalPredicted = maxArrivalOnGreen;
                        _PAOGTotalPredicted = pAOGUpstreamPredicted;
                    }
                }
            }
            //If downsteam only has detection
            else if (upstreamPCD.Count == 0 && downstreamPCD.Count > 0)
            {
                //if a bias was provided bias the results in the direction specified
                if (bias != 0)
                {
                    double upstreamBias = 1;
                    double downstreamBias = 1;
                    if (biasDirection == "Downstream")
                    {
                        downstreamBias = 1 + (bias / 100);
                    }
                    else
                    {
                        upstreamBias = 1 + (bias / 100);
                    }
                    //set the original values to compare against
                    double maxBiasArrivalOnGreen = (aOGDownstreamBefore * downstreamBias);
                    maxArrivalOnGreen = aOGDownstreamBefore + aOGUpstreamBefore;
                    //Add the total aog to the dictionary
                    resultsGraph.Add(0, maxArrivalOnGreen);
                    downstreamResultsGraph.Add(0, aOGDownstreamBefore * downstreamBias);
                    aOGDownstreamPredicted = aOGDownstreamBefore;
                    pAOGDownstreamPredicted = Math.Round(aOGDownstreamBefore / totalVolumeDownstream, 2)*100;
                    secondsAdded = 0;

                    for (int i = 1; i <= cycleTime; i++)
                    {
                        double totalBiasArrivalOnGreen = 0;
                        double totalArrivalOnGreen = 0;                      
                        double totalDownstreamAog = 0;

                        for (int index = 0; index < dates.Count; index++)
                        {
                            
                            downstreamPCD[index].LinkPivotAddSeconds(1);
                            totalBiasArrivalOnGreen += (downstreamPCD[index].TotalArrivalOnGreen * downstreamBias);
                            totalArrivalOnGreen = + (downstreamPCD[index].TotalArrivalOnGreen);
                            totalDownstreamAog += downstreamPCD[index].TotalArrivalOnGreen;
                        }
                        //Add the total aog to the dictionary
                        resultsGraph.Add(i, totalBiasArrivalOnGreen);
                        downstreamResultsGraph.Add(i, totalDownstreamAog);
                        if (totalBiasArrivalOnGreen > maxBiasArrivalOnGreen)
                        {
                            maxBiasArrivalOnGreen = totalBiasArrivalOnGreen;
                            maxArrivalOnGreen = totalArrivalOnGreen;
                            aOGDownstreamPredicted = totalDownstreamAog;
                            pAOGDownstreamPredicted = Math.Round(totalDownstreamAog / totalVolumeDownstream, 2)*100;
                            secondsAdded = i;
                        }
                    }
                    //Get the link totals
                    _AOGTotalPredicted = maxArrivalOnGreen;
                    _PAOGTotalPredicted = pAOGDownstreamPredicted;
                }
                //if no bias was provided
                else
                {
                    //set the original values to compare against
                    maxArrivalOnGreen = aOGDownstreamBefore;
                    //Add the total aog to the dictionary
                    resultsGraph.Add(0, maxArrivalOnGreen);
                    downstreamResultsGraph.Add(0, aOGDownstreamBefore);
                    aOGDownstreamPredicted = aOGDownstreamBefore;
                    pAOGDownstreamPredicted = Math.Round(aOGDownstreamBefore / totalVolumeDownstream, 2) * 100;
                    secondsAdded = 0;

                    for (int i = 1; i <= cycleTime; i++)
                    {

                        double totalArrivalOnGreen = 0;
                        double totalDownstreamAog = 0;

                        for (int index = 0; index < dates.Count; index++)
                        {
                            downstreamPCD[index].LinkPivotAddSeconds(1);
                            totalArrivalOnGreen = +(downstreamPCD[index].TotalArrivalOnGreen);
                            totalDownstreamAog += downstreamPCD[index].TotalArrivalOnGreen;
                        }
                        //Add the total aog to the dictionary
                        resultsGraph.Add(i, totalArrivalOnGreen);
                        downstreamResultsGraph.Add(i, totalDownstreamAog);
                        if (totalArrivalOnGreen > maxArrivalOnGreen)
                        {
                            maxArrivalOnGreen = totalArrivalOnGreen;
                            aOGDownstreamPredicted = totalDownstreamAog;
                            pAOGDownstreamPredicted = Math.Round(totalDownstreamAog / totalVolumeDownstream, 2)*100;
                            secondsAdded = i;
                        }
                    }
                    //Get the link totals
                    _AOGTotalPredicted = maxArrivalOnGreen;
                    _PAOGTotalPredicted = pAOGDownstreamPredicted;
                }
              

                //pAOGDownstreamPredicted = downstreamPCD.PercentArrivalOnGreen;
                //aOGDownstreamPredicted = downstreamPCD.TotalArrivalOnGreen;

                //downstreamAfterPCDPath = CreateChart(downstreamPCD, startDate, endDate, downSignalLocation,
                //            "after", chartLocation);
            }
            GetNewResultsChart(chartLocation);

        }

        private void GetNewResultsChart(string chartLocation)
        {
            
            Chart chart = new Chart();
           
            //Set the chart properties
            chart.ImageType = ChartImageType.Jpeg;
            chart.Height = 650;
            chart.Width = 1100;
            chart.ImageStorageMode = ImageStorageMode.UseImageLocation;

            //Set the chart title
            Title title = new Title();
            title.Text = "Max Arrivals On Green By Second";
            title.Font = new Font(FontFamily.GenericSansSerif, 20);
            chart.Titles.Add(title);

            //Create the chart legend
            Legend chartLegend = new Legend();
            chartLegend.Name = "MainLegend";
            //chartLegend.LegendStyle = LegendStyle.Table;
            chartLegend.Docking = Docking.Left;
            //chartLegend.CustomItems.Add(Color.Blue, "AoG - Arrival On Green");
            //chartLegend.CustomItems.Add(Color.Blue, "GT - Green Time");
            //chartLegend.CustomItems.Add(Color.Maroon, "PR - Platoon Ratio");
            //LegendCellColumn a = new LegendCellColumn();
            //a.ColumnType = LegendCellColumnType.Text;
            //a.Text = "test";
            //chartLegend.CellColumns.Add(a);
            chart.Legends.Add(chartLegend);


            //Create the chart area
            ChartArea chartArea = new ChartArea();
            chartArea.Name = "ChartArea1";
            chartArea.AxisY.Minimum = 0;
            chartArea.AxisY.Title = "Arrivals On Green";
            chartArea.AxisY.TitleFont = new Font(FontFamily.GenericSansSerif, 20);
            chartArea.AxisY.LabelAutoFitStyle = LabelAutoFitStyles.None;
            chartArea.AxisY.LabelStyle.Font = new Font(FontFamily.GenericSansSerif, 20);


            chartArea.AxisX.Minimum = 0;
            chartArea.AxisX.Title = "Adjustment(seconds)";
            chartArea.AxisX.TitleFont = new Font(FontFamily.GenericSansSerif, 20);
            chartArea.AxisX.Interval = 10;
            chartArea.AxisX.LabelAutoFitStyle = LabelAutoFitStyles.None;
            chartArea.AxisX.LabelStyle.Font = new Font(FontFamily.GenericSansSerif, 20);
            
            chart.ChartAreas.Add(chartArea);

            //Add the line series
            Series lineSeries = new Series();
            lineSeries.ChartType = SeriesChartType.Line;
            lineSeries.Color = Color.Black;
            lineSeries.Name = "Total AOG";
            lineSeries.XValueType = ChartValueType.Int32;
            lineSeries.BorderWidth = 5;
            chart.Series.Add(lineSeries);
            
            foreach(KeyValuePair<int,double> d in ResultsGraph)
            chart.Series["Total AOG"].Points.AddXY(
                            d.Key,
                            d.Value);

            //Add the line series
            Series downstreamLineSeries = new Series();
            downstreamLineSeries.ChartType = SeriesChartType.Line;
            downstreamLineSeries.Color = Color.Blue;
            downstreamLineSeries.Name = "Downstream AOG";
            downstreamLineSeries.XValueType = ChartValueType.Int32;
            downstreamLineSeries.BorderWidth = 3;
            chart.Series.Add(downstreamLineSeries);

            foreach (KeyValuePair<int, double> d in DownstreamResultsGraph)
                chart.Series["Downstream AOG"].Points.AddXY(
                                d.Key,
                                d.Value);

            //Add the line series
            Series upstreamLineSeries = new Series();
            upstreamLineSeries.ChartType = SeriesChartType.Line;
            upstreamLineSeries.Color = Color.Green;
            upstreamLineSeries.Name = "Upstream AOG";
            upstreamLineSeries.XValueType = ChartValueType.Int32;
            upstreamLineSeries.BorderWidth = 3;
            chart.Series.Add(upstreamLineSeries);

            foreach (KeyValuePair<int, double> d in UpstreamResultsGraph)
                chart.Series["Upstream AOG"].Points.AddXY(
                                d.Key,
                                d.Value);
            
                string chartName = "LinkPivot-" + signalId + upstreamApproachDirection +
                DownSignalId + DownstreamApproachDirection + 
                DateTime.Now.Day.ToString()+
                DateTime.Now.Hour.ToString() +
                DateTime.Now.Minute.ToString() +
                DateTime.Now.Second.ToString() +
                ".jpg";
            
                chart.SaveImage(chartLocation + @"LinkPivot\" + chartName,
                System.Web.UI.DataVisualization.Charting.ChartImageFormat.Jpeg);

            resultChartLocation = ConfigurationManager.AppSettings["ImageWebLocation"] +
                    @"LinkPivot/" + chartName;
            
        }

       
        /// <summary>
        /// Creates a pcd chart specific to the Link Pivot
        /// </summary>
        /// <param name="sp"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="location"></param>
        /// <param name="chartNameSuffix"></param>
        /// <param name="chartLocation"></param>
        /// <returns></returns>
        private string CreateChart(SignalPhase sp, DateTime startDate, DateTime endDate, string location,
            string chartNameSuffix, string chartLocation)
        {
            Chart chart = new Chart();
            //Display the PDC chart
            chart = GetNewChart(startDate, endDate, sp.Approach.SignalID, sp.Approach.ProtectedPhaseNumber, 
                sp.Approach.DirectionType.Description,
                    location, sp.Approach.IsProtectedPhaseOverlap, 150, 2000, false, 2);

            AddDataToChart(chart, sp, startDate, endDate, sp.Approach.SignalID, false, true);

            //Create the File Name
            string chartName = "LinkPivot-" +
                sp.Approach.SignalID +
                "-" +
                sp.Approach.ProtectedPhaseNumber.ToString() +
                "-" +
                startDate.Year.ToString() +
                startDate.ToString("MM") +
                startDate.ToString("dd") +
                startDate.ToString("HH") +
                startDate.ToString("mm") +
                "-" +
                endDate.Year.ToString() +
                endDate.ToString("MM") +
                endDate.ToString("dd") +
                endDate.ToString("HH") +
                endDate.ToString("mm-") +
                chartNameSuffix +
                ".jpg";



            //Save an image of the chart
            chart.SaveImage(chartLocation + chartName, System.Web.UI.DataVisualization.Charting.ChartImageFormat.Jpeg);
            return chartName;
        }

        /// <summary>
        /// Gets a new chart for the pcd Diagram
        /// </summary>
        /// <param name="graphStartDate"></param>
        /// <param name="graphEndDate"></param>
        /// <param name="signalId"></param>
        /// <param name="phase"></param>
        /// <param name="direction"></param>
        /// <param name="location"></param>
        /// <returns></returns>
        private Chart GetNewChart(DateTime graphStartDate, DateTime graphEndDate, string signalId,
            int phase, string direction, string location, bool isOverlap, double y1AxisMaximum,
            double y2AxisMaximum, bool showVolume, int dotSize)
        {
            double y = 0;
            Chart chart = new Chart();
            string extendedDirection = string.Empty;
            string movementType = "Phase";
            if (isOverlap)
            {
                movementType = "Overlap";
            }


            //Gets direction for the title
            switch (direction)
            {
                case "SB":
                    extendedDirection = "Southbound";
                    break;
                case "NB":
                    extendedDirection = "Northbound";
                    break;
                default:
                    extendedDirection = direction;
                    break;
            }

            //Set the chart properties
            chart.ImageType = ChartImageType.Jpeg;
            chart.Height = 650;
            chart.Width = 1100;
            chart.ImageStorageMode = ImageStorageMode.UseImageLocation;

            //Set the chart title
            Title title = new Title();
            title.Text = location + "Signal " + signalId.ToString() + " "
                + movementType + ": " + phase.ToString() +
                " " + extendedDirection + "\n" + graphStartDate.ToString("f") +
                " - " + graphEndDate.ToString("f");
            title.Font = new Font(FontFamily.GenericSansSerif, 20);
            chart.Titles.Add(title);

            //Create the chart legend
            //Legend chartLegend = new Legend();
            //chartLegend.Name = "MainLegend";
            ////chartLegend.LegendStyle = LegendStyle.Table;
            //chartLegend.Docking = Docking.Left;
            //chartLegend.CustomItems.Add(Color.Blue, "AoG - Arrival On Green");
            //chartLegend.CustomItems.Add(Color.Blue, "GT - Green Time");
            //chartLegend.CustomItems.Add(Color.Maroon, "PR - Platoon Ratio");
            ////LegendCellColumn a = new LegendCellColumn();
            ////a.ColumnType = LegendCellColumnType.Text;
            ////a.Text = "test";
            ////chartLegend.CellColumns.Add(a);
            //chart.Legends.Add(chartLegend);


            //Create the chart area
            ChartArea chartArea = new ChartArea();
            chartArea.Name = "ChartArea1";
            chartArea.AxisY.Maximum = y1AxisMaximum;           
            chartArea.AxisY.Minimum = 0;
            chartArea.AxisY.Title = "Cycle Time (Seconds) ";
            chartArea.AxisY.TitleFont = new Font(FontFamily.GenericSansSerif, 20);
            chartArea.AxisY.LabelAutoFitStyle = LabelAutoFitStyles.None;
            chartArea.AxisY.LabelStyle.Font = new Font(FontFamily.GenericSansSerif, 20);

            if (showVolume)
            {
                chartArea.AxisY2.Enabled = AxisEnabled.True;
                chartArea.AxisY2.MajorTickMark.Enabled = true;
                chartArea.AxisY2.MajorGrid.Enabled = false;
                chartArea.AxisY2.IntervalType = DateTimeIntervalType.Number;
                chartArea.AxisY2.Interval = 500;
                chartArea.AxisY2.Maximum = y2AxisMaximum;
                chartArea.AxisY2.Title = "Volume Per Hour ";
            }

            chartArea.AxisX.Title = "Time (Hour of Day)";
            chartArea.AxisX.TitleFont = new Font(FontFamily.GenericSansSerif, 20);
            chartArea.AxisX.Interval = 1;
            chartArea.AxisX.IntervalType = DateTimeIntervalType.Hours;
            chartArea.AxisX.LabelStyle.Format = "HH";
            chartArea.AxisX.LabelAutoFitStyle = LabelAutoFitStyles.None;
            chartArea.AxisX.LabelStyle.Font = new Font(FontFamily.GenericSansSerif, 20);
            
            //chartArea.AxisX.Minimum = 0;

            chartArea.AxisX2.Enabled = AxisEnabled.True;
            chartArea.AxisX2.MajorTickMark.Enabled = true;
            chartArea.AxisX2.IntervalType = DateTimeIntervalType.Hours;
            chartArea.AxisX2.LabelStyle.Format = "HH";
            chartArea.AxisX2.LabelAutoFitStyle = LabelAutoFitStyles.None;
            chartArea.AxisX2.Interval = 1;
            chartArea.AxisX2.LabelStyle.Font = new Font(FontFamily.GenericSansSerif, 20);
            //chartArea.AxisX.Minimum = 0;

            chart.ChartAreas.Add(chartArea);

            //Add the point series
            Series pointSeries = new Series();
            pointSeries.ChartType = SeriesChartType.Point;
            pointSeries.Color = Color.Black;
            pointSeries.Name = "Detector Activation";
            pointSeries.XValueType = ChartValueType.DateTime;
            pointSeries.MarkerSize = dotSize;
            chart.Series.Add(pointSeries);

            //Add the green series
            Series greenSeries = new Series();
            greenSeries.ChartType = SeriesChartType.Line;
            greenSeries.Color = Color.DarkGreen;
            greenSeries.Name = "Change to Green";
            greenSeries.XValueType = ChartValueType.DateTime;
            greenSeries.BorderWidth = 1;
            chart.Series.Add(greenSeries);

            //Add the yellow series
            Series yellowSeries = new Series();
            yellowSeries.ChartType = SeriesChartType.Line;
            yellowSeries.Color = Color.Yellow;
            yellowSeries.Name = "Change to Yellow";
            yellowSeries.XValueType = ChartValueType.DateTime;
            chart.Series.Add(yellowSeries);

            //Add the red series
            Series redSeries = new Series();
            redSeries.ChartType = SeriesChartType.Line;
            redSeries.Color = Color.Red;
            redSeries.Name = "Change to Red";
            redSeries.XValueType = ChartValueType.DateTime;
            chart.Series.Add(redSeries);

            //Add the red series
            Series volumeSeries = new Series();
            volumeSeries.ChartType = SeriesChartType.Line;
            volumeSeries.Color = Color.Black;
            volumeSeries.Name = "Volume Per Hour";
            volumeSeries.XValueType = ChartValueType.DateTime;
            volumeSeries.YAxisType = AxisType.Secondary;
            chart.Series.Add(volumeSeries);



            //Add points at the start and and of the x axis to ensure
            //the graph covers the entire period selected by the user
            //whether there is data or not
            chart.Series["Detector Activation"].Points.AddXY(graphStartDate, 0);
            chart.Series["Detector Activation"].Points.AddXY(graphEndDate, 0);
            return chart;
        }

        /// <summary>
        /// Adds data points to a graph with the series GreenLine, YellowLine, Redline
        /// and Points already added.
        /// </summary>
        /// <param name="chart"></param>
        /// <param name="signalPhase"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="signalId"></param>
        private void AddDataToChart(Chart chart, MOE.Common.Business.SignalPhase signalPhase, DateTime startDate,
            DateTime endDate, string signalId, bool showVolume, bool showArrivalOnGreen)
        {
            decimal totalDetectorHits = 0;
            decimal totalOnGreenArrivals = 0;
            decimal percentArrivalOnGreen = 0;

            foreach (MOE.Common.Business.Plan plan in signalPhase.Plans.PlanList)
            {
                if (plan.CycleCollection.Count > 0)
                {
                    foreach (MOE.Common.Business.Cycle pcd in plan.CycleCollection)
                    {
                        chart.Series["Change to Green"].Points.AddXY(
                            //pcd.StartTime,
                            pcd.GreenEvent,
                            pcd.GreenLineY);
                        chart.Series["Change to Yellow"].Points.AddXY(
                            //pcd.StartTime,
                            pcd.YellowEvent,
                            pcd.YellowLineY);
                        chart.Series["Change to Red"].Points.AddXY(
                            //pcd.StartTime, 
                            pcd.EndTime,
                            pcd.RedLineY);
                        totalDetectorHits += pcd.DetectorCollection.Count;
                        foreach (MOE.Common.Business.DetectorDataPoint detectorPoint in pcd.DetectorCollection)
                        {
                            chart.Series["Detector Activation"].Points.AddXY(
                                //pcd.StartTime, 
                                detectorPoint.TimeStamp,
                                detectorPoint.YPoint);
                            if (detectorPoint.YPoint > pcd.GreenLineY && detectorPoint.YPoint < pcd.RedLineY)
                            {
                                totalOnGreenArrivals++;
                            }
                        }
                    }
                }
            }

            if (showVolume)
            {
                foreach (MOE.Common.Business.Volume v in signalPhase.Volume.Items)
                {
                    chart.Series["Volume Per Hour"].Points.AddXY(v.XAxis, v.YAxis);
                }
            }

            //if arrivals on green is selected add the data to the chart
            if (showArrivalOnGreen)
            {
                if (totalDetectorHits > 0)
                {
                    percentArrivalOnGreen = (totalOnGreenArrivals / totalDetectorHits) * 100;
                }
                else
                {
                    percentArrivalOnGreen = 0;
                }
                Title title = new Title();
                title.Text = Math.Round(percentArrivalOnGreen).ToString() + "% AoG";
                 title.Font = new Font(FontFamily.GenericSansSerif, 20);
                 chart.Titles.Add(title);


                SetPlanStrips(signalPhase.Plans.PlanList, chart, startDate);
            }

            //Add Comment to chart



            //MOE.Common.Data.Signals.SPM_CommentDataTable commentTable = new MOE.Common.Data.Signals.SPM_CommentDataTable();
            //MOE.Common.Data.SignalsTableAdapters.SPM_CommentTableAdapter commentTA = new MOE.Common.Data.SignalsTableAdapters.SPM_CommentTableAdapter();
            //commentTA.FillByEntitybyChartType(commentTable, 4, signalId.ToString(), 2);

             //MOE.Common.Models.SPM db = new MOE.Common.Models.SPM();
             //var commentTable = from r in db.Comment
             //                      where r.ChartType == 4 && r.Entity == signalId && r.EntityType == 2
             //                      select r;



             //   if (commentTable.Count() > 0)
             //   {
             //   MOE.Common.Models.Comment comment = commentTable.FirstOrDefault();
             //   chart.Titles.Add(comment.Comment);
             //   chart.Titles[1].Docking = Docking.Bottom;
             //   chart.Titles[1].ForeColor = Color.Red;
            //}

            
        }


        /// <summary>
        /// Adds plan strips to the chart
        /// </summary>
        /// <param name="planCollection"></param>
        /// <param name="chart"></param>
        /// <param name="graphStartDate"></param>
        protected void SetPlanStrips(List<MOE.Common.Business.Plan> planCollection, Chart chart, DateTime graphStartDate)
        {
            int backGroundColor = 1;
            foreach (MOE.Common.Business.Plan plan in planCollection)
            {
                StripLine stripline = new StripLine();
                //Creates alternating backcolor to distinguish the plans
                if (backGroundColor % 2 == 0)
                {
                    stripline.BackColor = Color.FromArgb(120, Color.LightGray);
                }
                else
                {
                    stripline.BackColor = Color.FromArgb(120, Color.LightBlue);
                }

                //Set the stripline properties
                stripline.IntervalOffset = (plan.StartTime - graphStartDate).TotalHours;
                stripline.IntervalOffsetType = DateTimeIntervalType.Hours;
                stripline.Interval = 1;
                stripline.IntervalType = DateTimeIntervalType.Days;               
                stripline.StripWidth = (plan.EndTime - plan.StartTime).TotalHours;
                stripline.StripWidthType = DateTimeIntervalType.Hours;

                chart.ChartAreas["ChartArea1"].AxisX.StripLines.Add(stripline);

                //Add a corrisponding custom label for each strip
                CustomLabel Plannumberlabel = new CustomLabel();
                Plannumberlabel.FromPosition = plan.StartTime.ToOADate();
                Plannumberlabel.ToPosition = plan.EndTime.ToOADate();
                switch (plan.PlanNumber)
                {
                    case 254:
                        Plannumberlabel.Text = "Free";
                        break;
                    case 255:
                        Plannumberlabel.Text = "Flash";
                        break;
                    case 0:
                        Plannumberlabel.Text = "Unknown";
                        break;
                    default:
                        Plannumberlabel.Text = "Plan " + plan.PlanNumber.ToString();

                        break;
                }

                Plannumberlabel.ForeColor = Color.Black;
                Plannumberlabel.RowIndex = 3;
                chart.ChartAreas["ChartArea1"].AxisX2.CustomLabels.Add(Plannumberlabel);

                CustomLabel aogLabel = new CustomLabel();
                aogLabel.FromPosition = plan.StartTime.ToOADate();
                aogLabel.ToPosition = plan.EndTime.ToOADate();
                aogLabel.Text = plan.PercentArrivalOnGreen.ToString() + "% AoG\n" +
                    plan.PercentGreen.ToString() + "% GT";

                aogLabel.LabelMark = LabelMarkStyle.LineSideMark;
                aogLabel.ForeColor = Color.Blue;
                aogLabel.RowIndex = 2;
                chart.ChartAreas["ChartArea1"].AxisX2.CustomLabels.Add(aogLabel);

                CustomLabel statisticlabel = new CustomLabel();
                statisticlabel.FromPosition = plan.StartTime.ToOADate();
                statisticlabel.ToPosition = plan.EndTime.ToOADate();
                statisticlabel.Text =
                    plan.PlatoonRatio.ToString() + " PR";
                statisticlabel.ForeColor = Color.Maroon;
                statisticlabel.RowIndex = 1;
                chart.ChartAreas["ChartArea1"].AxisX2.CustomLabels.Add(statisticlabel);

                //CustomLabel PlatoonRatiolabel = new CustomLabel();
                //PercentGreenlabel.FromPosition = plan.StartTime.ToOADate();
                //PercentGreenlabel.ToPosition = plan.EndTime.ToOADate();
                //PercentGreenlabel.Text = plan.PlatoonRatio.ToString() + " PR";
                //PercentGreenlabel.ForeColor = Color.Black;
                //PercentGreenlabel.RowIndex = 1;
                //chart.ChartAreas["ChartArea1"].AxisX2.CustomLabels.Add(PercentGreenlabel);

                //Change the background color counter for alternating color
                backGroundColor++;

            }
        }

        
    }
}
