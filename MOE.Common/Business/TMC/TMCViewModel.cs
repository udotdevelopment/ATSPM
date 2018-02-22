using System;
using System.Collections.Generic;
using System.Linq;
using MOE.Common.Models;
using MOE.Common.Models.Repositories;

namespace MOE.Common.Business.TMC
{
    public class TMCViewModel
    {
        public List<RecordFooter> Footers = new List<RecordFooter>();
        public List<RecordHeader> Headers = new List<RecordHeader>();
        public List<string> ImageLocations = new List<string>();
        public List<int> LaneTypeColSpans = new List<int>();
        public List<RecordHeader> PeakHourHeaders = new List<RecordHeader>();
        public List<Record> PeakHourValues = new List<Record>();
        public List<Record> Records = new List<Record>();

        public TMCViewModel(bool showLaneVolumes, bool showDataTable)
        {
            ShowLaneVolumes = showLaneVolumes;
            ShowDataTable = showDataTable;
            var laneRepository =
                LaneTypeRepositoryFactory.Create();
            var directionRepository =
                DirectionTypeRepositoryFactory.Create();
            var movementRepository =
                MovementTypeRepositoryFactory.Create();
            Headers = new List<RecordHeader>();
            Records = new List<Record>();
            Footers = new List<RecordFooter>();
            LaneTypes = laneRepository.GetAllLaneTypes();
            DirectionTypes = directionRepository.GetAllDirections();
            MovementTypes = movementRepository.GetAllMovementTypes();
            PeakHourFactor = 0;
        }

        public string ShowMetricUrlJavascript { get; set; } = string.Empty;
        public bool ShowLaneVolumes { get; set; }
        public KeyValuePair<DateTime, int> PeakHour { get; set; }
        public bool ShowDataTable { get; set; }
        public int BinSize { get; set; }
        public List<LaneType> LaneTypes { get; set; }
        public List<DirectionType> DirectionTypes { get; set; }
        public List<MovementType> MovementTypes { get; set; }
        public List<DateTime> BinStartTimes { get; set; }
        public double PeakHourFactor { get; set; }


        public void PopulateViewModel(List<TMCData> tMCData, int binSize)
        {
            BinSize = binSize;
            SetBinStartTimes(tMCData);
            AddTimeStampsToRecordList(tMCData);
            foreach (var l in LaneTypes)
            {
                foreach (var d in DirectionTypes)
                {
                    foreach (var m in MovementTypes)
                        if (tMCData.Where(t => t.LaneType == l.Description && t.Direction == d.Description
                                               && t.MovementType == m.Abbreviation).Count() > 0)
                            SumLaneCountsByLaneTypeDirectionAndMovement(l, d, m, tMCData);
                    if (tMCData.Where(t => t.LaneType == l.Description
                                           && t.Direction == d.Description).Count() > 0)
                        AddDirectionTypeTotals(l, d, tMCData);
                }
                if (tMCData.Where(t => t.LaneType == l.Description).Count() > 0)
                    AddLaneTypeTotals(l, tMCData);
            }
            SetHeaderSpans();
            if (BinSize == 15)
            {
                PeakHour = FindPeakHour(tMCData);
                SetPeakHourFactor(tMCData);
                AddPeakHourDataToList(tMCData);
            }
        }

        private void SetPeakHourFactor(List<TMCData> tMCData)
        {
            try
            {
                if (tMCData
                        .Where(t => t.LaneType == "Vehicle"
                                    && t.Timestamp >= PeakHour.Key
                                    && t.Timestamp < PeakHour.Key.AddHours(1))
                        .Count() > 0)
                {
                    var maxCount = tMCData
                        .Where(t => t.LaneType == "Vehicle"
                                    && t.Timestamp >= PeakHour.Key
                                    && t.Timestamp < PeakHour.Key.AddHours(1))
                        .GroupBy(t => t.Timestamp)
                        .Select(t => new {Id = t.Key, Count = t.Sum(y => y.Count)})
                        .Max(t => t.Count);
                    double denominator = 4 * maxCount;
                    if (denominator != 0)
                        PeakHourFactor = Math.Round(PeakHour.Value / denominator, 2);
                }
            }
            catch
            {
                throw new Exception("Error Setting Peak Hour");
            }
        }

        private void SetBinStartTimes(List<TMCData> tMCData)
        {
            BinStartTimes = (from r in tMCData
                select r.Timestamp).Distinct().OrderBy(r => r).ToList();
        }

        private void AddPeakHourDataToList(List<TMCData> tMCData)
        {
            PeakHourValues.Add(new Record());
            PeakHourValues[0].Values = new List<Tuple<int, string>>();
            foreach (var d in DirectionTypes)
            {
                foreach (var m in MovementTypes)
                    if (tMCData.Where(t => t.LaneType == "Vehicle"
                                           && t.Direction == d.Description
                                           && t.MovementType == m.Abbreviation
                                           && t.Timestamp >= PeakHour.Key
                                           && t.Timestamp < PeakHour.Key.AddHours(1)).Count() > 0)
                        PeakHourValues[0].Values.Add(new Tuple<int, string>(tMCData.Where(t => t.LaneType == "Vehicle"
                                                                                               && t.Direction ==
                                                                                               d.Description
                                                                                               && t.MovementType ==
                                                                                               m.Abbreviation
                                                                                               && t.Timestamp >=
                                                                                               PeakHour.Key
                                                                                               && t.Timestamp <
                                                                                               PeakHour.Key.AddHours(1))
                            .Sum(t => t.Count), string.Empty));
                if (tMCData.Where(t => t.LaneType == "Vehicle"
                                       && t.Direction == d.Description
                                       && t.Timestamp >= PeakHour.Key
                                       && t.Timestamp < PeakHour.Key.AddHours(1)).Count() > 0)
                    PeakHourValues[0].Values.Add(new Tuple<int, string>(tMCData.Where(t => t.LaneType == "Vehicle"
                                                                                           && t.Direction ==
                                                                                           d.Description
                                                                                           && t.Timestamp >=
                                                                                           PeakHour.Key
                                                                                           && t.Timestamp <
                                                                                           PeakHour.Key.AddHours(1))
                        .Sum(t => t.Count), "TMCDirectionTotalColumn"));
            }
        }

        private KeyValuePair<DateTime, int> FindPeakHour(List<TMCData> tMCData)
        {
            var totalVolume = new KeyValuePair<DateTime, int>(DateTime.MinValue, 0);

            foreach (var date in BinStartTimes)
            {
                var tempVolume = tMCData
                    .Where(t =>
                        t.Timestamp >= date
                        && t.Timestamp < date.AddHours(1)
                        && t.LaneType == "Vehicle")
                    .Sum(t => t.Count);
                if (tempVolume > totalVolume.Value)
                    totalVolume = new KeyValuePair<DateTime, int>(date, tempVolume);
            }
            return totalVolume;
        }

        private void SetHeaderSpans()
        {
            Headers[0].SetSpans();
            Headers[1].SetSpans();
            PeakHourHeaders[0].SetSpans();
        }

        private void AddDirectionTypeTotals(LaneType l, DirectionType d, List<TMCData> tMCData)
        {
            AddHeaderInfo(l.Description, l.Description, d.Description, d.Description + l.Description, "Total",
                "DirectionTotal");
            if (l.Description == "Vehicle")
                AddPeakHourHeaderInfo(d.Description, d.Description + l.Description, "Total", "DirectionTotal");
            foreach (var r in Records)
                r.Values.Add(new Tuple<int, string>(tMCData
                    .Where(t => t.Timestamp == r.Timestamp
                                && t.LaneType == l.Description
                                && t.Direction == d.Description)
                    .Sum(t => t.Count), "DirectionTotal"));
            AddFooterValue(tMCData
                .Where(t => t.LaneType == l.Description
                            && t.Direction == d.Description)
                .Sum(t => t.Count));
        }

        private void AddLaneTypeTotals(LaneType l, List<TMCData> tMCData)
        {
            AddHeaderInfo(l.Description, l.Description, string.Empty, l.Description, l.Description + " Total",
                "LaneTypeTotal");
            foreach (var r in Records)
                r.Values.Add(new Tuple<int, string>(tMCData
                    .Where(t => t.Timestamp == r.Timestamp
                                && t.LaneType == l.Description)
                    .Sum(t => t.Count), "LaneTypeTotal"));
            AddFooterValue(tMCData
                .Where(t => t.LaneType == l.Description)
                .Sum(t => t.Count));
        }

        private void AddFooterValue(int value)
        {
            Footers[0].Values.Add(value);
        }

        private void AddTimeStampsToRecordList(List<TMCData> tMCData)
        {
            var Timestamps = (from r in tMCData
                select r.Timestamp).Distinct().ToList();
            foreach (var d in Timestamps)
                Records.Add(new Record {Timestamp = d});
            Footers.Add(new RecordFooter {Title = "Total"});
        }

        private void AddHeaderRows()
        {
            Headers.Add(new RecordHeader());
            Headers.Add(new RecordHeader());
            Headers.Add(new RecordHeader());
            PeakHourHeaders.Add(new RecordHeader());
            PeakHourHeaders.Add(new RecordHeader());
        }

        private void SumLaneCountsByLaneTypeDirectionAndMovement(LaneType lane, DirectionType direction,
            MovementType movement, List<TMCData> tMCData)
        {
            AddHeaderInfo(lane.Description, lane.Description,
                direction.Description, direction.Description + lane.Description,
                movement.Abbreviation, movement.Abbreviation);
            if (lane.Description == "Vehicle")
                AddPeakHourHeaderInfo(direction.Description, direction.Description + lane.Description,
                    movement.Abbreviation, movement.Abbreviation);
            foreach (var r in Records)
                r.Values.Add(new Tuple<int, string>(tMCData
                    .Where(t => t.LaneType == lane.Description
                                && t.Direction == direction.Description
                                && t.MovementType == movement.Abbreviation
                                && t.Timestamp == r.Timestamp)
                    .Sum(t => t.Count), string.Empty));
            AddFooterValue(tMCData
                .Where(t => t.LaneType == lane.Description
                            && t.Direction == direction.Description
                            && t.MovementType == movement.Abbreviation)
                .Sum(t => t.Count));
        }

        private void AddPeakHourHeaderInfo(string row2, string uniqueIdentifier2, string row3, string uniqueIdentifier3)
        {
            PeakHourHeaders[0].Values.Add(new Tuple<string, string>(row2, uniqueIdentifier2));
            PeakHourHeaders[1].Values.Add(new Tuple<string, string>(row3, uniqueIdentifier3));
        }

        private void AddHeaderInfo(string row1, string uniqueIdentifier1, string row2, string uniqueIdentifier2,
            string row3, string uniqueIdentifier3)
        {
            if (Headers.Count == 0)
                AddHeaderRows();
            Headers[0].Values.Add(new Tuple<string, string>(row1, uniqueIdentifier1));
            Headers[1].Values.Add(new Tuple<string, string>(row2, uniqueIdentifier2));
            Headers[2].Values.Add(new Tuple<string, string>(row3, uniqueIdentifier3));
        }
    }

    public class RecordHeader
    {
        public List<int> Spans = new List<int>();
        public List<Tuple<string, string>> Values = new List<Tuple<string, string>>();

        public void SetSpans()
        {
            var uniqueValues = Values.Distinct().ToList();
            foreach (var v in uniqueValues)
                Spans.Add(Values.Where(value => value.Item2 == v.Item2).Count());
            Values = uniqueValues;
        }
    }

    public class Record
    {
        public List<Tuple<int, string>> Values = new List<Tuple<int, string>>();
        public DateTime Timestamp { get; set; }
    }

    public class RecordFooter
    {
        public List<int> Values = new List<int>();
        public string Title { get; set; }
    }
}