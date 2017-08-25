using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace MOE.Common.Business
{
    public class SignalPhaseCollection
    {
        

        List<SignalPhase> signalPhaseList = new List<SignalPhase>();
        public List<SignalPhase> SignalPhaseList
        {
            get { return signalPhaseList; }
        }


        //public SignalPhaseCollection( DateTime startDate, DateTime endDate, string signalID,
        //    bool showVolume, int binSize, int metricTypeID)
        //{
        //    MOE.Common.Models.Repositories.ISignalsRepository repository =
        //        MOE.Common.Models.Repositories.SignalsRepositoryFactory.Create();
        //    var signal = repository.GetSignalBySignalID(signalID);   
        //    var detectors = signal.GetDetectorsForSignal();
        //     if (detectors.Count > 0)
        //    {
        //        foreach (Models.Detector detector in detectors)
        //        {
        //            int detChannel = detector.DetChannel;
        //            Double offset = detector.GetOffset();
        //            String direction = detector.Approach.DirectionType.Description;
        //            bool isOverlap = detector.Approach.IsProtectedPhaseOverlap;

        //            //Get the phase
        //            MOE.Common.Business.SignalPhase signalPhase = new MOE.Common.Business.SignalPhase(
        //                startDate, endDate, detector.Approach, showVolume, binSize, metricTypeID);
        //            this.SignalPhaseList.Add(signalPhase);                    
        //        }
        //    }
        //}


        public SignalPhaseCollection(DateTime startDate, DateTime endDate, string signalID,
            bool showVolume, int binSize, int metricTypeID)
        {
            MOE.Common.Models.Repositories.ISignalsRepository repository =
                MOE.Common.Models.Repositories.SignalsRepositoryFactory.Create();
            var signal = repository.GetSignalBySignalID(signalID);
     
            List<Models.Approach> approaches = signal.GetApproachesForSignalThatSupportMetric(metricTypeID);
            if (signal.Approaches != null && approaches.Count > 0)
            {
                Parallel.ForEach(approaches, approach =>
                //foreach (Models.Approach approach in approaches)
                {
                    String direction = approach.DirectionType.Description;
                    bool isOverlap = approach.IsProtectedPhaseOverlap;
                    int phaseNumber = approach.ProtectedPhaseNumber;
                    //double offset = approach.GetOffset();

                    //Get the phase
                    MOE.Common.Business.SignalPhase signalPhase = new MOE.Common.Business.SignalPhase(
                        startDate, endDate, approach, showVolume, binSize, metricTypeID);

                    //try not to add the same direction twice
                    var ExsitingPhases = from MOE.Common.Business.SignalPhase phase in this.SignalPhaseList
                                         where phase.Approach.DirectionType.Description == signalPhase.Approach.DirectionType.Description
                                         select phase;

                    if (ExsitingPhases.Count() < 1)
                    {
                        this.SignalPhaseList.Add(signalPhase);
                    }

                });
                this.signalPhaseList = signalPhaseList.OrderBy(s => s.Approach.ProtectedPhaseNumber).ToList();
            }
        }

 
        // TODO: When we re-implement volume export, this entire section needs to be reworked.
        //private Data.MOE.VolumeExportDataTable CreateVolumeExportTable(string signalId, DateTime startDate,
        //    DateTime endDate, int binSize)
        //{
                        
        //    Data.MOE.VolumeExportDataTable table = new Data.MOE.VolumeExportDataTable();
        //    DateTime dtVariable = startDate.AddMinutes(binSize);
        //    while (dtVariable < endDate)
        //    {
        //        table.AddVolumeExportRow(signalId, dtVariable, 0, 0, 0, 0, 0, 0, 0, 0);
        //        dtVariable = dtVariable.AddMinutes(binSize);
        //    }

        //    return table;
        //}


    //public Data.MOE.VolumeExportDataTable GetVolumeExportData(DateTime startDate,
    //DateTime endDate, string signalId, //string region, 
    //    int binSize, SortedDictionary<int, string> directions)
    //{
    //    int D1vol = 0;
    //    int D2vol = 0;
    //    int D3vol = 0;
    //    int D4vol = 0;
    //    DateTime D1time = new DateTime();
    //    DateTime D2time = new DateTime();
    //    DateTime D3time = new DateTime();
    //    DateTime D4time = new DateTime();
    //    SortedDictionary<DateTime, int> D1volumes = new SortedDictionary<DateTime, int>();
    //    SortedDictionary<DateTime, int> D2volumes = new SortedDictionary<DateTime, int>();
    //    SortedDictionary<DateTime, int> D3volumes = new SortedDictionary<DateTime, int>();
    //    SortedDictionary<DateTime, int> D4volumes = new SortedDictionary<DateTime, int>();
    //    Data.MOE.VolumeExportDataTable volumeTable = CreateVolumeExportTable(signalId, startDate,
    //        endDate, binSize);

    //    int D1TotalVolume = 0;
    //    int D2TotalVolume = 0;
    //    int D3TotalVolume = 0;
    //    int D4TotalVolume = 0;
        
    //    foreach (Business.SignalPhase approachDirection in this.SignalPhaseList)
    //    {
    //        if (approachDirection.Volume.Items.Count > 0)
    //        {
    //            foreach (Business.Volume v in approachDirection.Volume.Items)
    //            {

    //                //add Direction1 volumes
    //                if (directions.ContainsKey(1) &&
    //                    approachDirection.Approach.DirectionType.Description == directions[1])
    //                {
    //                    //Add the volumes and times to a collection so we can use them later
    //                    D1volumes.Add(v.XAxis, v.DetectorCount);
    //                    D1TotalVolume = (D1TotalVolume + v.DetectorCount);
    //                    DataRow[] rows = volumeTable.Select("DateTime = '" + v.XAxis.ToString()+"'");
    //                    rows[0]["Direction1Volume"] = v.DetectorCount;
    //                }

    //                //add Direction2 volumes
    //                else if (directions.ContainsKey(2) &&
    //                    approachDirection.Approach.DirectionType.Description == directions[2])
    //                {
    //                    //Add the volumes and times to a collection so we can use them later
    //                    D2volumes.Add(v.XAxis, v.DetectorCount);
    //                    D2TotalVolume = (D2TotalVolume + v.DetectorCount);
    //                    DataRow[] rows = volumeTable.Select("DateTime = '" + v.XAxis.ToString() + "'");
    //                    rows[0]["Direction2Volume"] = v.DetectorCount;                            
    //                }

    //                //add Direction3 volumes
    //                else if (directions.ContainsKey(3) &&
    //                    approachDirection.Approach.DirectionType.Description == directions[3])
    //                {
    //                    //Add the volumes and times to a collection so we can use them later
    //                    D3volumes.Add(v.XAxis, v.DetectorCount);
    //                    D3TotalVolume = (D3TotalVolume + v.DetectorCount);
    //                    DataRow[] rows = volumeTable.Select("DateTime = '" + v.XAxis.ToString() + "'");
    //                    rows[0]["Direction3Volume"] = v.DetectorCount;
    //                }

    //                //add Direction4 volumes
    //                else if (directions.ContainsKey(4) &&
    //                    approachDirection.Approach.DirectionType.Description == directions[4])
    //                {
    //                    //Add the volumes and times to a collection so we can use them later
    //                    D4volumes.Add(v.XAxis, v.DetectorCount);
    //                    D4TotalVolume = (D4TotalVolume + v.DetectorCount);
    //                    DataRow[] rows = volumeTable.Select("DateTime = '" + v.XAxis.ToString() + "'");
    //                    rows[0]["Direction4Volume"] = v.DetectorCount;
    //                }
    //            }
    //            //add ratios


    //            //Match the times in the dir1 colleciton to the dir2 collection so we can get a ratio
    //            //of the values collected at the same point in time.
    //            foreach (KeyValuePair<DateTime, int> volRow in D1volumes)
    //            {
    //                D2vol = (from k in D2volumes
    //                            where DateTime.Compare(k.Key, volRow.Key) == 0
    //                            select k.Value).FirstOrDefault();

    //                D1vol = volRow.Value;
    //                D1time = volRow.Key;

    //                if (D1vol > 0 && D2vol > 0)
    //                {
    //                    //ratio the values
    //                    double D1DFactor = Convert.ToDouble(D1vol) / ((Convert.ToDouble(D2vol) + Convert.ToDouble(D1vol)));
    //                    DataRow[] rows = volumeTable.Select("DateTime = '" + D1time.ToString()+"'");
    //                    rows[0]["Direction1Directional"] = D1DFactor;
    //                }

    //            }

    //            //Match the times in the dir2 colleciton to the dir1 collection so we can get a ratio
    //            //of the values collected at the same point in time.
    //            foreach (KeyValuePair<DateTime, int> volRow in D2volumes)
    //            {
    //                D1vol = (from k in D1volumes
    //                            where DateTime.Compare(k.Key, volRow.Key) == 0
    //                            select k.Value).FirstOrDefault();

    //                D2vol = volRow.Value;
    //                D2time = volRow.Key;

    //                if (D2vol > 0 && D2vol > 0)
    //                {
    //                    //ratio the values
    //                    double D2DFactor = Convert.ToDouble(D2vol) / ((Convert.ToDouble(D2vol) + Convert.ToDouble(D1vol)));
    //                    DataRow[] rows = volumeTable.Select("DateTime = '" + D2time.ToString() + "'");
    //                    rows[0]["Direction2Directional"] = D2DFactor;
    //                }

    //            }

    //            //Match the times in the dir1 colleciton to the dir2 collection so we can get a ratio
    //            //of the values collected at the same point in time.
    //            foreach (KeyValuePair<DateTime, int> volRow in D3volumes)
    //            {
    //                D4vol = (from k in D4volumes
    //                         where DateTime.Compare(k.Key, volRow.Key) == 0
    //                         select k.Value).FirstOrDefault();

    //                D3vol = volRow.Value;
    //                D3time = volRow.Key;

    //                if (D3vol > 0 && D4vol > 0)
    //                {
    //                    //ratio the values
    //                    double D3DFactor = Convert.ToDouble(D3vol) / ((Convert.ToDouble(D4vol) + Convert.ToDouble(D3vol)));
    //                    DataRow[] rows = volumeTable.Select("DateTime = '" + D3time.ToString() + "'");
    //                    rows[0]["Direction3Directional"] = D3DFactor;
    //                }

    //            }

    //            //Match the times in the dir2 colleciton to the dir1 collection so we can get a ratio
    //            //of the values collected at the same point in time.
    //            foreach (KeyValuePair<DateTime, int> volRow in D2volumes)
    //            {
    //                D3vol = (from k in D3volumes
    //                         where DateTime.Compare(k.Key, volRow.Key) == 0
    //                         select k.Value).FirstOrDefault();

    //                D4vol = volRow.Value;
    //                D4time = volRow.Key;

    //                if (D4vol > 0 && D4vol > 0)
    //                {
    //                    //ratio the values
    //                    double D4DFactor = Convert.ToDouble(D4vol) / ((Convert.ToDouble(D4vol) + Convert.ToDouble(D3vol)));
    //                    DataRow[] rows = volumeTable.Select("DateTime = '" + D4time.ToString() + "'");
    //                    rows[0]["Direction4Directional"] = D4DFactor;
    //                }

    //            }

    //        }

    //    }


    //    return volumeTable;
    //}


    }
}
